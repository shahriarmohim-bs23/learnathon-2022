using System.ComponentModel.DataAnnotations;

namespace backend_task3
{
    public class Minimamage : ValidationAttribute
    {
        int _minage;
        public Minimamage(int minage)
        {
            _minage = minage;
        }
        public override bool IsValid(object value)
        {
            DateTime date;
            if (DateTime.TryParse(value.ToString(), out date))
            {
                return date.AddYears(_minage) < DateTime.Now;
            }
            return false;
        }
    }
}
