using System;
using System.Net.Security;
using CprSoap.CprService;

namespace CprSoap
{
    internal class CprSoapClient
    {
        private static void Main(string[] args)
        {
            // Check input
            if (args.Length == 0 || args[0].Length != 10)
            {
                Console.WriteLine(string.Format("Usage: {0} <cpr number>", AppDomain.CurrentDomain.FriendlyName));
                return;
            }

            // Create a new client
            var client = new PersonBasicInformationService_1_portTypeClient();

            // Messages should be signed, but not encrypted
            client.Endpoint.Contract.ProtectionLevel = ProtectionLevel.Sign;

            try
            {
                client.Open();

                PersonBasicInformationResponseType response = client.PersonBasicInformationService_1_operation(args[0]);

                if (response.PersonBasicInformationStructure == null)
                {
                    // Lookup failed
                    Console.WriteLine(string.Format("Cpr lookup failed: {0}", response.ReturnStructure.ReturnMessageText));
                    return;
                }

                // Retrieve e.g. name and postal address from the response
                AddressPostalType addressPostal =
                    ((AddressCompleteType)
                     ((DanishAddressStructureType) response.PersonBasicInformationStructure.Item).
                         Item).AddressPostal;

                Console.WriteLine(
                    string.Format(
                        "Person name: {0}\nDistrict name: {1}\nPostal code: {2}\nStreet name: {3}\nHouse number: {4}\nFloor: {5}\nDoor: {6}",
                        response.PersonBasicInformationStructure.AddresseeName, addressPostal.DistrictName,
                        addressPostal.PostCodeIdentifier,
                        addressPostal.StreetName, addressPostal.StreetBuildingIdentifier,
                        addressPostal.FloorIdentifier, addressPostal.SuiteIdentifier));

                Console.ReadLine();
            }
            catch (Exception e)
            {
                Console.WriteLine(string.Format("Exception occurred: {0}", e));
                client.Abort();
            }
            finally
            {
                client.Close();
            }
        }
    }
}