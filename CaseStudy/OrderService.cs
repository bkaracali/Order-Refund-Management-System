using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaseStudy
{
    public class OrderService
    {
        private readonly List<Orders> _orders;
        private readonly List<OrderedItems> _orderedItems;
        private readonly List<PaymentMethods> _paymentMethods;

        public OrderService(List<Orders> orders, List<OrderedItems> orderedItems, List<PaymentMethods> paymentMethods)
        {
            _orders = orders;
            _orderedItems = orderedItems;
            _paymentMethods = paymentMethods;
        }

        // Kapıda ödeme olan siparişleri filtreleyen method
        public IQueryable<Orders> GetOrdersWithCashOnDelivery()
        {
            var cashOnDeliveryId = _paymentMethods
                .FirstOrDefault(p => p.PaymentType.Equals("Kapıda", StringComparison.OrdinalIgnoreCase))?.PaymentMethodId;

            if (cashOnDeliveryId == null)
            {
                throw new Exception("Kapıda ödeme türü bulunamadı.");
            }

            return _orders
                .Where(o => o.PaymentMethod == cashOnDeliveryId.ToString())
                .AsQueryable();
        }

        // Kapıda ödeme siparişlerine ait ürünleri filtreleyen method
        public IQueryable<OrderedItems> GetOrderedItemsForCashOnDelivery()
        {
            var cashOnDeliveryOrders = GetOrdersWithCashOnDelivery();

            // OrderedItems listesini filtrele
            var filteredOrderedItems = _orderedItems
                .Where(item => cashOnDeliveryOrders.Any(order => order.OrderId == item.OrderId))
                .AsQueryable();

            return filteredOrderedItems;
        }

        // Siparişlerin toplam değerini hesaplayıp güncelleyen method
        public void UpdateOrderValues()
        {
            var orderedItems = GetOrderedItemsForCashOnDelivery();

            var groupedByOrder = orderedItems
                .GroupBy(item => item.OrderId)
                .Select(group => new
                {
                    OrderId = group.Key,
                    MeanValue = group.Sum(item => item.Quantity * item.Price) / group.Sum(item => item.Quantity)  // group.Average doğru sonuç vermedi.
                });

            foreach (var orderSummary in groupedByOrder)
            {
                Console.WriteLine($"OrderId: {orderSummary.OrderId}, Mean Value: {orderSummary.MeanValue}");

            }
        }

        public List<Refund> checkExist(int orderId, int CustomerId, List<Refund> testData)
        {

            // Eşleşen Refund var mı diye kontrol et
            var existingRefund = testData
                .FirstOrDefault(r => r.OrderId == orderId && r.CustomerId == CustomerId);

            if (existingRefund != null)
            {
                // Eğer eşleşme varsa, birden fazla refund aynı sipariş için oluşturulamaz

                Console.WriteLine("Aynı sipariş için birden fazla iade oluşturulamaz.");

                return testData;
            }
            else
            {
                // Eğer eşleşme yoksa, yeni Refund nesnesi oluştur
                testData.Add(new Refund { OrderId = orderId, CustomerId = CustomerId });

                Console.WriteLine("İade oluşturuldu.");

                return testData;
            }

        }
    }

}
