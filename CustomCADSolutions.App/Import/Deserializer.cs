using System.ComponentModel.DataAnnotations;
using System.Text;
using CustomCADSolutions.Infrastructure.Data;
using Newtonsoft.Json;
using CustomCADSolutions.Infrastructure.Data.Models;
using CustomCADSolutions.Infrastructure.Data.Models.Enums;

namespace CustomCADSolutions.App.Import
{
    public class Deserializer
    {
        public static string ImportPharmacies(CADContext context, string xmlString)
        {
            StringBuilder sb = new();
            /*

            var pharmacyDTOs = XmlHelper.Deserialize<ImportPharmacyDTO[]>(xmlString, "Pharmacies");
            List<Pharmacy> pharmacies = new();

            foreach (ImportPharmacyDTO pharmacyDTO in pharmacyDTOs)
            {
                if (!IsValid(pharmacyDTO))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                bool validIsNonStop = bool.TryParse(pharmacyDTO.IsNonStop, out bool isNonStop);
                if (!validIsNonStop)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Regex regexPharmacyPhone = new(@"\(\d{3}\) \d{3}-\d{4}");
                if (!regexPharmacyPhone.IsMatch(pharmacyDTO.PhoneNumber))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Pharmacy pharmacy = new Pharmacy
                {
                    Name = pharmacyDTO.Name,
                    IsNonStop = isNonStop,
                    PhoneNumber = pharmacyDTO.PhoneNumber
                };

                foreach (ImportMedicineDTO medicineDTO in pharmacyDTO.Medicines)
                {
                    if (!IsValid(medicineDTO))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    if (!DateTime.TryParseExact(medicineDTO.ProductionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime productionDate))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    if (!DateTime.TryParseExact(medicineDTO.ExpiryDate, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime expiryDate))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    if (productionDate >= expiryDate)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    if (pharmacy.Medicines.Any(m => m.Name == medicineDTO.Name && m.Producer == medicineDTO.Producer))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    Medicine medicine = new()
                    {
                        Name = medicineDTO.Name,
                        Price = medicineDTO.Price,
                        Category = (Category)medicineDTO.Category,
                        ProductionDate = productionDate,
                        ExpiryDate = expiryDate,
                        Producer = medicineDTO.Producer,
                    };
                    pharmacy.Medicines.Add(medicine);
                }

                pharmacies.Add(pharmacy);
                sb.AppendLine(string.Format(SuccessfullyImportedPharmacy, pharmacy.Name, pharmacy.Medicines.Count));
            }

            context.Pharmacies.AddRange(pharmacies);
            context.SaveChanges();

            */
            return sb.ToString();
        }

        public static void ImportCategories(CADContext context, string jsonString)
        {
            ImportCadDTO[] cadDTOs = JsonConvert.DeserializeObject<ImportCadDTO[]>(jsonString)!;
            List<Cad> cads = new();

            foreach (ImportCadDTO cadDTO in cadDTOs)
            {
                if (!IsValid(cadDTO)) continue;

                Cad cad = new()
                {
                    Name = cadDTO.Name,
                    Url = cadDTO.URL,
                    CreationDate = DateTime.Now,
                    Category = cadDTO.Category,
                };

                cads.Add(cad);
            }

            context.CADs.AddRange(cads);
            context.SaveChanges();
        }

        private static bool IsValid(object dto)
        {
            var validationContext = new ValidationContext(dto);
            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(dto, validationContext, validationResult, true);
        }
    }
}
