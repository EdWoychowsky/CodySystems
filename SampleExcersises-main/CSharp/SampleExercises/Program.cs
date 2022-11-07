using Newtonsoft.Json;
using SimpleDataManagement.Models;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json;
using System.Text.Json.Serialization;
using static System.Net.Mime.MediaTypeNames;

var dataSourcesDirectory = Path.Combine(Environment.CurrentDirectory, "DataSources");
var personsFilePath = Path.Combine(dataSourcesDirectory, "Persons_20220824_00.json");
var organizationsFilePath = Path.Combine(dataSourcesDirectory, "Organizations_20220824_00.json");
var vehiclesFilePath = Path.Combine(dataSourcesDirectory, "Vehicles_20220824_00.json");
var addressesFilePath = Path.Combine(dataSourcesDirectory, "Addresses_20220824_00.json");

//Quick test to ensure that all files are where they should be :)
foreach (var path in new[] { personsFilePath, organizationsFilePath, vehiclesFilePath, addressesFilePath })
{
    if (!File.Exists(path))
        throw new FileNotFoundException(path);
}

//TODO: Start your exercise here. Do not forget about answering Test #9 (Handled slightly different)
// Reminder: Collect the data from each file. Hint: You could use Newtonsoft's JsonConvert or Microsoft's JsonSerializer
List<Address> addresses = new List<Address>();
List<Organization> organizations = new List<Organization>();
List<Person> persons = new List<Person>();
List<Vehicle> vehicles = new List<Vehicle>();

try
{
    foreach (var path in new[] { personsFilePath, organizationsFilePath, vehiclesFilePath, addressesFilePath })
        using (StreamReader sr = new StreamReader(path))
        {
            string json = sr.ReadToEnd();

            if (path.Contains("Address"))
                addresses = System.Text.Json.JsonSerializer.Deserialize<List<Address>>(json);
            else
                if (path.Contains("Organization"))
                    organizations = System.Text.Json.JsonSerializer.Deserialize<List<Organization>>(json);
                else
                    if (path.Contains("Persons"))
                        persons = System.Text.Json.JsonSerializer.Deserialize<List<Person>>(json);
                    else
                        if (path.Contains("Vehicle"))
                            vehicles = System.Text.Json.JsonSerializer.Deserialize<List<Vehicle>>(json);
                        else
                            throw new NotImplementedException("Get data from file");
        }
}
catch (NotImplementedException)
{
    throw new NotImplementedException("Get data from file");
}

//Test #1: Do all files have entities? (True / False)
try
{
    Console.WriteLine("Test #1: Do all files have entities? (True / False)");

    Console.WriteLine((addresses.Select(x => x.EntityId).Count() != 0 
                                             && organizations.Select(x => x.EntityId).Count() != 0 
                                             && persons.Select(x => x.EntityId).Count() != 0 
                                             && vehicles.Select(x => x.EntityId).Count() != 0).ToString().ToUpper());
}
catch (NotImplementedException)
{
    throw new NotImplementedException("Complete Test #1");
}

//Test #2: What is the total count for all entities?
int addressTally = addresses.Select(x => x.EntityId).Count();
int organizationTally = organizations.Select(x => x.EntityId).Count();
int personTally = persons.Select(x => x.EntityId).Count();
int vehicleTally = vehicles.Select(x => x.EntityId).Count();
int tally = addressTally + organizationTally + personTally + vehicleTally;

try
{
    Console.WriteLine("\n\nTest #2: What is the total count for all entities?");

    Console.WriteLine(tally);
}
catch (NotImplementedException)
{
    throw new NotImplementedException("Complete Test #2");
}

//Test #3: What is the count for each type of Entity? Person, Organization, Vehicle, and Address
try
{
    Console.WriteLine("\n\nTest #3: What is the count for each type of Entity? Person, Organization, Vehicle, and Address");

    Console.WriteLine(string.Format("Person: {0}", personTally));
    Console.WriteLine(string.Format("Organization: {0}", organizationTally));
    Console.WriteLine(string.Format("Vehicle: {0}", vehicleTally));
    Console.WriteLine(string.Format("Address: {0}", addressTally));
}
catch (NotImplementedException)
{
    throw new NotImplementedException("Complete Test #3");
}

//Test #4: Provide a breakdown of entities which have associations in the following manor:
//         -Per Entity Count
//         - Total Count
try
{
    Console.WriteLine("\n\nTest #4: Provide a breakdown of entities which have associations in the following manor:");

    addressTally = addresses.SelectMany(x => x.Associations).Select(x => x.EntityId).Count();
    organizationTally = organizations.SelectMany(x => x.Associations).Select(x => x.EntityId).Count();
    personTally = persons.SelectMany(x => x.Associations).Select(x => x.EntityId).Count();
    vehicleTally = vehicles.SelectMany(x => x.Associations).Select(x => x.EntityId).Count();
    tally = addressTally + organizationTally + personTally + vehicleTally;

    Console.WriteLine(string.Format("Person associations: {0}", personTally));
    Console.WriteLine(string.Format("Organization associations: {0}", organizationTally));
    Console.WriteLine(string.Format("Vehicle associations: {0}", vehicleTally));
    Console.WriteLine(string.Format("Address associations: {0}", addressTally));
    Console.WriteLine(string.Format("Total associations: {0}", tally));
}
catch (NotImplementedException)
{
    throw new NotImplementedException("Complete Test #4");
}

//Test #5: Provide the vehicle detail that is associated to the address "4976 Penelope Via South Franztown, NH 71024"?
//         StreetAddress: "4976 Penelope Via"
//         City: "South Franztown"
//         State: "NH"
//         ZipCode: "71024"
try
{
    Console.WriteLine("\n\nTest #5: Provide the vehicle detail that is associated to the address \"4976 Penelope Via South Franztown, NH 71024\"?");
    String[] address = addresses.Where(x => x.StreetAddress.Contains("4976 Penelope Via")
                                            && x.City.Contains("South Franztown")
                                            && x.State.Contains("NH")
                                            && x.ZipCode.Contains("71024")).SelectMany(x => x.Associations)
                                                                            .Where(x => x.EntityType.Contains("Vehicle"))
                                                                            .Select(x => x.EntityId)
                                                                            .ToArray();

    foreach (Vehicle vehicle in vehicles.Where(x => address.Contains(x.EntityId)).ToList())
    {
        Console.WriteLine(string.Format("Id: {0}", vehicle.EntityId));
        Console.WriteLine(string.Format("Make: {0}", vehicle.Make));
        Console.WriteLine(string.Format("Model: {0}", vehicle.Model));
        Console.WriteLine(string.Format("Color: {0}", vehicle.Color));
        Console.WriteLine(string.Format("Year: {0}", vehicle.Year));
        Console.WriteLine(string.Format("Vehicle Type: {0}", vehicle.VehicleType));
        Console.WriteLine(string.Format("Plate Number: {0}", vehicle.PlateNumber));
        Console.WriteLine(string.Format("State: {0}", vehicle.State));
        Console.WriteLine(string.Format("Vin: {0}", vehicle.Vin));
    }
}
catch (NotImplementedException)
{
    throw new NotImplementedException("Complete Test #5");
}

//Test #6: What person(s) are associated to the organization "thiel and sons"?
try
{
    Console.WriteLine("\n\nTest #6: What person(s) are associated to the organization \"thiel and sons\"?");

    List<Person> person = (from p in persons
                           from a in p.Associations
                           join o in organizations on a.EntityId equals o.EntityId
                           where p.Associations.Any(y => y.EntityType.Contains("Organization"))
                           && o.Name.ToLower().Contains("thiel and sons")
                           select p).ToList();

    if (person.Count() == 0)
        Console.WriteLine("None");
    else
        foreach (Person data in person)
        {
            Console.WriteLine(string.Format("First Name: {0}", data.FirstName));
            Console.WriteLine(string.Format("Last Name: {0}", data.LastName));
            Console.WriteLine(string.Format("Middle Name: {0}", data.MiddleName));
            Console.WriteLine(string.Format("Date Of Birth: {0}", data.DateOfBirth));
            Console.WriteLine(string.Format("Entity Id: {0}\n", data.EntityId));
        }
}
catch (NotImplementedException)
{
    throw new NotImplementedException("Complete Test #6");
}

//Test #7: How many people have the same first and middle name?
try
{
    Console.WriteLine("\n\nTest #7: How many people have the same first and middle name?");

    List<Person> same = (from p0 in persons
                         join p1 in persons on new { p0.EntityId, Name = p0.FirstName.ToLower() } equals new { p1.EntityId, Name = p1.MiddleName.ToLower() }
                         select p0).ToList();

    Console.WriteLine(same.Count());
}
catch (NotImplementedException)
{
    throw new NotImplementedException("Complete Test #7");
}


//Test #8: Provide a breakdown of entities where the EntityId contains "B3" in the following manor:
//         -Total count by type of Entity
//         - Total count of all entities
try
{
    Console.WriteLine("\n\nTest #8: Provide a breakdown of entities where the EntityId contains \"B3\" in the following manor:");
    Console.WriteLine("-Total count by type of Entity");
    Console.WriteLine("-Total count of all entities");

    var b3 = (from x in addresses
              where x.EntityId.ToLower().Contains("b3")
              select new
              {
                  x.EntityId,
                  EntityType = "Address"
              }).Concat(from x in organizations
                        where x.EntityId.ToLower().Contains("b3")
                        select new
                        {
                            x.EntityId,
                            EntityType = "Organization"
                        }).Concat(from x in persons
                                  where x.EntityId.ToLower().Contains("b3")
                                  select new
                                  {
                                      x.EntityId,
                                      EntityType = "Person"
                                  }).Concat(from x in vehicles
                                            where x.EntityId.ToLower().Contains("b3")
                                            select new
                                            {
                                                x.EntityId,
                                                EntityType = "Vehicle"
                                            });

    foreach (string entity in new string[] { "Person", "Vehicle", "Organization", "Address" })
        Console.WriteLine(string.Format("{0} total: {1}", entity, b3.Where(x => x.EntityType.Contains(entity)).Count()));

    Console.WriteLine(string.Format("Total: {0}", b3.Count()));
}
catch (NotImplementedException)
{
    throw new NotImplementedException("Complete Test #8");
}

Console.WriteLine("\n\nTest #9: Improvements:");
Console.WriteLine("I would change the structure somewhat by eliminating the Association class. Each Model would have its own string list");
Console.WriteLine("that would refer to the other associated models. This would make writing queries easier.");
Console.WriteLine("\nFor example new Address model/class see code.");

public class NewAddress
{
    public string StreetAddress { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public string ZipCode { get; set; }
    public string EntityId { get; set; }

    IList<string>? _organizations;
    IList<string>? _persons;
    IList<string>? _vehicles;
    public IList<string> Organizations
    {
        get
        {
            if (_organizations == null)
                _organizations = new List<string>();

            return _organizations;
        }
        set
        {
            _organizations = value;
        }
    }
    public IList<string> Persons
    {
        get
        {
            if (_persons == null)
                _persons = new List<string>();

            return _persons;
        }
        set
        {
            _persons = value;
        }
    }
    public IList<string> Vehicles
    {
        get
        {
            if (_vehicles == null)
                _vehicles = new List<string>();

            return _vehicles;
        }
        set
        {
            _vehicles = value;
        }
    }
}