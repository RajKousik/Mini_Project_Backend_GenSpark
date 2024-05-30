using System.Runtime.Serialization;

namespace StudentManagementApplicationAPI.Services.Student_Service
{
    [Serializable]
    public class InvalidRechargeAmount : Exception
    {
        private string msg;
        public InvalidRechargeAmount()
        {
            msg = "Invalid Recharge Amount";
        }

        public InvalidRechargeAmount(string message) : base(message)
        {
            msg = message;
        }

        public override string Message => msg;
    }
}