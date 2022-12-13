namespace Payment.Domain.ValueObjects
{
    public class Account
    {
        public Account(string name, long accountNumber, string ifscCode)
        {
            Name = name;
            AccountNumber = accountNumber;
            IfscCode = ifscCode;
        }

        public string Name { get; }
        public long AccountNumber { get; }
        public string IfscCode { get; }
    }
}