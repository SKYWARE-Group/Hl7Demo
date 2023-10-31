using NHapi.Base.Model;
using NHapi.Base.Parser;
using NHapi.Model.V25.Group;
using NHapi.Model.V25.Message;

// Ignore Spelling: ver hl

namespace Hl7Demo
{

    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Order messages processing example");

            string[] files = new[] { "order-1.txt", "order-2.txt" };
            string appBasePath = Path.GetDirectoryName(Environment.ProcessPath)!;

            var parser = new PipeParser();
            ParserOptions opts = new() { NonGreedyMode = true }; // IMPORTANT! See https://github.com/nHapiNET/nHapi/pull/240

            foreach (string file in files)
            {

                Console.WriteLine("=========================");
                Console.WriteLine($"File: {file}");
                Console.WriteLine("=========================");

                string payload = File.ReadAllText(Path.Combine(appBasePath, "examples", file));
                IMessage message = parser.Parse(payload, opts);
                if (message is not OML_O21 oml) throw new ApplicationException($"Unsupported type of message ({message.GetStructureName()})");

                // MSH
                Console.WriteLine($"Sender: {oml.MSH.SendingApplication.NamespaceID} ({oml.MSH.SendingFacility.NamespaceID})");

                // SFT
                if (oml.SFTRepetitionsUsed > 0)
                {
                    Console.WriteLine($"Application info: {oml.GetSFT(0).SoftwareProductName}, ver.{oml.GetSFT(0).SoftwareCertifiedVersionOrReleaseNumber}"); // Optional segment
                }

                // PATIENT group
                // PID
                Console.WriteLine("Patient:");
                Console.WriteLine($"   Given name: {oml.PATIENT.PID.GetPatientName(0)?.GivenName?.Value}"); // At least one repetition will be present
                Console.WriteLine($"   Middle name: {oml.PATIENT.PID.GetPatientName(0)?.SecondAndFurtherGivenNamesOrInitialsThereof?.Value}"); // Optional
                Console.WriteLine($"   Family name: {oml.PATIENT.PID.GetPatientName(0)?.FamilyName?.Surname?.Value}"); // Optional
                Console.WriteLine($"   Identifiers:");
                for (int pidIndex = 0; pidIndex < oml.PATIENT.PID.PatientIdentifierListRepetitionsUsed; pidIndex++)
                {
                    // At least LAB^PI will be provided (PK in LIS iLab)
                    Console.WriteLine($"      {oml.PATIENT.PID.GetPatientIdentifierList(pidIndex).AssigningAuthority?.NamespaceID}" +
                        $"-{oml.PATIENT.PID.GetPatientIdentifierList(pidIndex).IdentifierTypeCode?.Value}:" +
                        $" {oml.PATIENT.PID.GetPatientIdentifierList(pidIndex).IDNumber}");
                }

                // PATIENT VISIT group
                // PV1
                Console.WriteLine("Visit:");
                Console.WriteLine($"   Location: {oml.PATIENT.PATIENT_VISIT.PV1.AssignedPatientLocation?.PointOfCare?.Value}");
                Console.WriteLine($"   Location code: {oml.PATIENT.PATIENT_VISIT.PV1.AssignedPatientLocation?.Facility?.NamespaceID?.Value}");
                Console.WriteLine($"   NRN: {oml.PATIENT.PATIENT_VISIT.PV1.VisitNumber?.IDNumber?.Value}"); // Optional, only in case it is NHIS referral


                // ORDER group(s) - Ordered/Cancelled examinations
                Console.WriteLine($"Ordered/Cancelled examinations:");
                foreach (OML_O21_ORDER orderGroup in oml.ORDERs)
                {

                    // ORC
                    Console.WriteLine($"   Action: {orderGroup.ORC.OrderControl.Value}"); // Table 0119 - Order control codes
                    Console.WriteLine($"   Placer order number: {orderGroup.ORC.PlacerOrderNumber?.EntityIdentifier?.Value} @ {orderGroup.ORC.PlacerOrderNumber?.NamespaceID?.Value}"); // Consider unique as composite key including namespace!
                    if (orderGroup.ORC.OrderingProviderRepetitionsUsed > 0) // Optional, only in cases where ordering physician is known
                    {
                        Console.WriteLine($"   Referring doctor UIN: {orderGroup.ORC.GetOrderingProvider(0)?.IDNumber?.Value} ({orderGroup.ORC.GetOrderingProvider(0)?.FamilyName?.Surname?.Value} {orderGroup.ORC.GetOrderingProvider(0)?.GivenName?.Value})");

                    }

                    // OBSERVATION REQUEST group
                    // OBR
                    Console.WriteLine($"   Examination (LOINC code): {orderGroup.OBSERVATION_REQUEST?.OBR?.UniversalServiceIdentifier?.Identifier?.Value}");
                    Console.WriteLine($"   Examination (Name): {orderGroup.OBSERVATION_REQUEST?.OBR?.UniversalServiceIdentifier?.Text?.Value}");

                    Console.WriteLine("------------");
                }


                Console.WriteLine();

            }

        }
    }
}