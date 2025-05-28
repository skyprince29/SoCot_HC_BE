using SoCot_HC_BE.Model.BaseModels;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace SoCot_HC_BE.Model
{
    public class NonCommunicableDisease: AuditInfo
    {
        [Key]
        public Guid Id { get; set; }

        [ForeignKey("Person")]
        public Guid? PatientId { get; set; }
        public Person? Patient { get; set; }

        public Guid? FamilyHistoryId { get; set; }
        public virtual FamilyHistory? FamilyHistory { get; set; }


        //[Display(Name = "Waist circumference(cm)")]
        public int Waist { get; set; }

        //First reading
        public Guid? FirstVitalSignId { get; set; }
        public virtual VitalSign? FirstVitalSign { get; set; }

        //Second reading
        public Guid? SecondVitalSignId { get; set; }
        public virtual VitalSign? SecondVitalSign { get; set; }


        //// converted to vitalsign
        ///        //obesity
        ////[DisplayName("Height(cm)")]
        //public int Height { get; set; }
        ////[DisplayName("Weight(kg)")]
        //public int Weight { get; set; }
        //public string BMI { get; set; } = String.Empty;

        ////BP
        ////[Display(Name = "Systolic 1st reading")]
        //public int Systolic1st { get; set; }

        ////[Display(Name = "Diastolic 1st reading")]
        //public int Diastolic1st { get; set; }
        ////[Display(Name = "Systolic 2nd reading")]
        //public int Systolic2nd { get; set; }
        ////[Display(Name = "Diastolic 2nd reading")]
        //public int Diastolic2nd { get; set; }

        public string AverageBP { get; set; } = String.Empty;

        public string Smoking { get; set; } = String.Empty;

        //[Display(Name = "AAlcohol Intake")]
        public string AlcoholIntake { get; set; } = String.Empty;

        public bool ExcessiveAlcoholIntake { get; set; } = false;

        public bool HighFatSalt { get; set; } = false;

        //fiber intake
        public bool Vegetable { get; set; } = false;

        public bool Fruits { get; set; } = false;

        //physical activity
        public bool PhysicalActivity { get; set; } = false;

        //Presence or abesence of Diabetes  
        public string withDiabetes { get; set; } = String.Empty;
        public bool Polyphagia { get; set; } = false;
        public bool Polydipsia { get; set; } = false;
        public bool Polyuria { get; set; } = false;

        //Presence of Urine Ketones/Protein
        public bool withKetones { get; set; } = false;

        public int Ketones { get; set; } = 0;
        public DateTime? KetonesDateTaken { get; set; } = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById("Singapore Standard Time"));

        public bool withProtein { get; set; } = false;

        public int UrineProtein { get; set; } = 0;

        public DateTime? UrineProteinDateTaken { get; set; } = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById("Singapore Standard Time"));

        //Raised blood glucose/lipids
        public bool Glucose { get; set; } = false;
        public int FBS_RBS { get; set; }

        public DateTime? GlucoseDateTaken { get; set; } = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById("Singapore Standard Time"));

        public bool Lipids { get; set; } = false;

        public int TotalCholesterol { get; set; }
        public DateTime? LipidsDateTaken { get; set; } = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById("Singapore Standard Time"));


        //Probable Angina, heart attack, stroke
        public bool AnginaHeart { get; set; } = false;

        public bool hasStrokeTIA { get; set; } = false;

        public string riskLevel { get; set; } = String.Empty;

        //Questions
        public string NCDQ1 { get; set; } = String.Empty;
        public string NCDQ2 { get; set; } = String.Empty;
        public string NCDQ3 { get; set; } = String.Empty;
        public string NCDQ4 { get; set; } = String.Empty;
        public string NCDQ5 { get; set; } = String.Empty;
        public string NCDQ6 { get; set; } = String.Empty;
        public string NCDQ7 { get; set; } = String.Empty;
        public string NCDQ8 { get; set; } = String.Empty;


        public DateTime? DateAssed { get; set; } = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById("Singapore Standard Time"));

    }
}
