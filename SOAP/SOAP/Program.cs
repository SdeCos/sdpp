using ServicioPaises;
namespace SOAP
{
    internal class Program
    {
        static void Main(string[] args)
        {
            CountryInfoServiceSoapTypeClient cliente = new CountryInfoServiceSoapTypeClient(CountryInfoServiceSoapTypeClient.EndpointConfiguration.CountryInfoServiceSoap12);
            Console.WriteLine(cliente.CountryFlag("ARG"));
            Console.ReadLine();
        }
    }
}
