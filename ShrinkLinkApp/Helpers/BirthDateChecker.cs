namespace ShrinkLinkApp.Helpers
{
    public class BirthDateChecker
    {
        public int Check(DateTime? birthDate, HttpContext context)
        {

            if (birthDate == null)
                return 0; //false

            var diff = (DateTime.Now).Subtract((DateTime)birthDate);
            var age = diff.TotalDays / 365;
            if (age <= 17)
                return 2; //for adults only

            if (birthDate <= DateTime.Now && age <= 120)
                return 1; //true

            return 0;  //false
        }
    }
}
