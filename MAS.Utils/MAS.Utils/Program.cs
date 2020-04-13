namespace MAS.Utils
{
    using System;
    using System.Reflection;

    using log4net;

    internal class Program
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private static void Main()
        {
            // Configures logging
            log4net.Config.XmlConfigurator.Configure();

            Log.Info("----------------Starting program----------------");

            try
            {
                var allDefensivePropertySets = MASUtils.ProcessAllDefensivePropertySets();

                var ownship = MASUtils.ProcessDefensivePropertySet(Prop1.Blade, Prop2.Fire, Prop3.Physical);
                var bestOwn = ownship.GetBestOffensivePropertySets();

                var blank = allDefensivePropertySets.Clone()
                    .EvaluateOffensiveCondition(Prop1.Blade, Prop2.None, Prop3.None, Comparator.eq, 0)
                    .EvaluateOffensiveCondition(Prop1.None, Prop2.Earth, Prop3.None, Comparator.eq, 0)
                    .EvaluateOffensiveCondition(Prop1.Blade, Prop2.None, Prop3.Magical, Comparator.eq, 0)
                    .EvaluateOffensiveCondition(Prop1.Blunt, Prop2.None, Prop3.Physical, Comparator.eq, -2);
                var bestBlank = blank.GetBestOffensivePropertySets();

                var daten = allDefensivePropertySets.Clone()
                    .EvaluateOffensiveCondition(Prop1.Blade, Prop2.None, Prop3.Magical, Comparator.eq, -1)
                    .EvaluateOffensiveCondition(Prop1.Blunt, Prop2.None, Prop3.Physical, Comparator.eq, 1)
                    .EvaluateOffensiveCondition(Prop1.Blade, Prop2.Fire, Prop3.None, Comparator.eq, 0)
                    .EvaluateOffensiveCondition(Prop1.Gun, Prop2.None, Prop3.None, Comparator.le, 0)
                    .EvaluateOffensiveCondition(Prop1.Blade, Prop2.None, Prop3.None, Comparator.eq, 0);
                var bestDaten = daten.GetBestOffensivePropertySets();

                var terra = allDefensivePropertySets.Clone()
                    .EvaluateOffensiveCondition(Prop1.Blade, Prop2.Water, Prop3.None, Comparator.eq, 1)
                    .EvaluateOffensiveCondition(Prop1.Blunt, Prop2.None, Prop3.Physical, Comparator.eq, -1)
                    .EvaluateOffensiveCondition(Prop1.None, Prop2.Earth, Prop3.None, Comparator.eq, -1)
                    .EvaluateOffensiveCondition(Prop1.None, Prop2.None, Prop3.Physical, Comparator.eq, -2);
                var bestTerra = terra.GetBestOffensivePropertySets();

                var grey = allDefensivePropertySets.Clone()
                    .EvaluateOffensiveCondition(Prop1.None, Prop2.None, Prop3.Physical, Comparator.eq, 0)
                    .EvaluateOffensiveCondition(Prop1.Blade, Prop2.None, Prop3.None, Comparator.eq, 0)
                    .FilterBy(Prop1.Blade, null, Prop3.Physical);
                var bestGrey = grey.GetBestOffensivePropertySets();

                MASUtils.PrintHistograms(allDefensivePropertySets);
                MASUtils.PrintHistograms(ownship, name: "OWNSHIP", printBestOffensiveProperties: true);
                MASUtils.PrintHistograms(blank, name: "BLANK", printBestOffensiveProperties: true);
                MASUtils.PrintHistograms(daten, name: "DATEN", printBestOffensiveProperties: true);
                MASUtils.PrintHistograms(terra, name: "TERRA", printBestOffensiveProperties: true);
                MASUtils.PrintHistograms(grey, name: "GREY", printBestOffensiveProperties: true);
            }
            catch (Exception e)
            {
                Log.Error("Failed to process properties", e);
            }

            Console.WriteLine("Press Enter to quit.");
            Console.ReadLine();
        }
    }
}