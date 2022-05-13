using Insurance.Api.Domain.Enums;
using System.Collections.Generic;

namespace Insurance.Api.Domain
{
    public class OrderEntity
    {
        public OrderEntity()
        {
            Items = new List<OrderItemEntity>();
        }
        public List<OrderItemEntity> Items { get; set; }
        public float InsuranceCost()
        {
            bool addCameraInsurance = false;
            float totalInsuranceVal = 0;
            Items.ForEach(x =>
            {
                totalInsuranceVal += x.InsuranceCost();
                if (x.Product is DigitaslCameraEntity)
                {
                    addCameraInsurance = true;
                }
            });

            if (addCameraInsurance) totalInsuranceVal += 500;

            return totalInsuranceVal;
        }
    }
}
