using System;
using System.Collections.Generic;
using System.Linq;

namespace SprintPlanner
{
    class SprintPlanner
    {
        private static double lastSprintNumWorkingDays;
        private static Dictionary<string, double> lastSprintPto = new Dictionary<string, double>();
        private static double numPointsPulledInLastSprint;
        private static double lastSprintVelocity;
        private static double lastSprintPointsPerHour;

        private static double numWorkingDays;
        private static Dictionary<string,double> thisSprintPto = new Dictionary<string, double>();
        private static double staffedCapacity;
        private static double averageVelocity;

        public static void Main(string[] args)
        {
            var morePtoExists = true;

            Console.WriteLine("How many points were pulled in last sprint: ");
            numPointsPulledInLastSprint = double.Parse(Console.ReadLine());

            Console.WriteLine("How my points were completed last sprint: ");
            lastSprintVelocity = double.Parse(Console.ReadLine());

            Console.WriteLine("How many work days were there last sprint: ");
            lastSprintNumWorkingDays = double.Parse(Console.ReadLine());

            while (morePtoExists)
            {
                Console.WriteLine("Enter last sprint's team PTO as: <name>,<numDaysPTO>. Blank when finished.");
                var ptoResponse = Console.ReadLine();
                if (string.IsNullOrEmpty(ptoResponse))
                {
                    morePtoExists = false;
                }
                else
                {
                    var teamMember = ptoResponse.Split(',').First();
                    var ptoDays = ptoResponse.Split(',')[1];
                    lastSprintPto.Add(teamMember, double.Parse(ptoDays));
                }
            }

            morePtoExists = true;

            Console.WriteLine("How many work days are there this sprint: ");
            numWorkingDays = double.Parse(Console.ReadLine());

            while (morePtoExists)
            {
                Console.WriteLine("Enter team PTO as: <name>,<numDaysPTO>. Blank when finished.");
                var ptoResponse = Console.ReadLine();
                if (string.IsNullOrEmpty(ptoResponse))
                {
                    morePtoExists = false;
                }
                else
                {
                    var teamMember = ptoResponse.Split(',').First();
                    var ptoDays = ptoResponse.Split(',')[1];
                    thisSprintPto.Add(teamMember, double.Parse(ptoDays));
                }
            }

            lastSprintPointsPerHour = CalculatePointsPerHour(lastSprintNumWorkingDays, lastSprintPto, lastSprintVelocity);
            averageVelocity = CalculatePointsPerHour(numWorkingDays, thisSprintPto);
            PrintInfo();
        }

        private static double CalculatePointsPerHour(double numWorkDaysInSprint, Dictionary<string, double> ptoInfo, double numPoints)
        {
            var workHours = 40 * numWorkDaysInSprint;
            var hrs = ptoInfo.Sum(p => p.Value).ToString();
            var hoursOfPto = double.Parse(hrs);
            var hoursOfUtilization = workHours - hoursOfPto;
            return (numPoints / hoursOfUtilization);
        }
        private static double CalculatePointsPerHour(double numWorkDaysInSprint, Dictionary<string, double> ptoInfo)
        {
            var workHours = 40 * numWorkDaysInSprint;
            var hrs = ptoInfo.Sum(p => p.Value).ToString();
            var hoursOfPto = double.Parse(hrs);
            var hoursOfUtilization = workHours - hoursOfPto;
            staffedCapacity = hoursOfUtilization / workHours;

            return lastSprintPointsPerHour * hoursOfUtilization;
        }

        private static void PrintInfo()
        {
            Console.WriteLine($"Number of points pulled in last sprint: {numPointsPulledInLastSprint}");
            Console.WriteLine($"Num points completed last sprint: {lastSprintVelocity}");
            Console.WriteLine($"Last sprint points per hour: {lastSprintPointsPerHour}");

            Console.WriteLine("----------------------------------------------------------------------");

            Console.WriteLine($"Number of points to pull in this sprint: {averageVelocity}");
            Console.WriteLine($"The team will be operating at {Math.Round(staffedCapacity * 100)}% capacity this sprint. Below is the PTO being taken");

            foreach (var entry in thisSprintPto)
            {
                Console.WriteLine(entry.Key + " : " + entry.Value);
            }
        }
    }
}
