namespace Payment.Domain.Core
{
    /// <summary>
    /// Core domain will be place where we need to concentrate all business related logics.
    /// The given demo Bank, Payment domains are anemic in nature and don't have much logic, that why it is just bunch of
    /// props and builder defined here. In actual world, the real domain will be thicker and will have plenty if business implementations   
    /// </summary>
    public class Bank
    {
        private Bank()
        {
        }

        public int Id { get; private set; }
        public string IfscCode { get; private set; }
        public string Name { get; private set; }


        /// <summary>
        /// Builder will be way to construct domain to ensure all required validations/ steps are done
        /// before return in to consumer  
        /// </summary>
        public class Builder
        {
            private readonly Bank _bank;

            public Builder()
            {
                _bank = new Bank();
            }

            public Builder WithBankId(int bankId)
            {
                _bank.Id = bankId;
                return this;
            }

            public Builder WithIfscCode(string ifscCode)
            {
                _bank.IfscCode = ifscCode;
                return this;
            }

            public Builder WithName(string name)
            {
                _bank.Name = name;
                return this;
            }

            public Bank Build()
            {
                //Validate(_bank);
                return _bank;
            }

            private static void Validate(Bank bank)
            {
                // usually we have business validations which will be triggered when construct a domain as domain
                // should not be construct in an invalid state.We can implement those validation here.
                // As this is a sample anemic domain, we don't have any such behaviour and validation
                // but in real world we will have plenty of those
            }
        }
    }
}