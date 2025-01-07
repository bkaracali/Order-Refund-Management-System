
/* "Bir şirket, müşterilerine online olarak ürün satışı yapmaktadır. Müşteriler, ürünleri sepete ekleyip ödeme işlemini gerçekleştirdikten sonra, 
* ürünleri kapıda ödeme seçeneğini de seçebilmektedir. Bu şirket için, kapıda ödeme seçeneğini kullanan müşterilerin sepete ekledikleri ürünlerin ortalama değerini bulmak istenmektedir. 
* Bu veriyi bulmak için nasıl bir yöntem önerirsiniz?"                                             
*/

/*“Aynı şirket, müşterilerine, geçmiş sipariş detaylarını uygulama ya da web sitesi üzerinden 
vermektedir. Müşterinin bir siparişi iptal etmek ve para iadesi almak istediği durumlarda, aynı sipariş 
için birden fazla iptal talebi geldiğinde müşteriye çift iade problemi ortaya çıkabilmektedir. Bu 
problemi çözmek için nasıl bir yöntem önerirsiniz*/

using CaseStudy;

var orders = new List<Orders>
        {
            new Orders { OrderId = 1, CustomerId = 101, PaymentMethod = "1" },
            new Orders { OrderId = 2, CustomerId = 102, PaymentMethod = "2" },
            new Orders { OrderId = 3, CustomerId = 101, PaymentMethod = "1" }
        };

var orderedItems = new List<OrderedItems>
        {
            new OrderedItems { OrderedItemdId = 1, OrderId = 1, ProductId = 1001, Quantity = 2, Price = 50 },
            new OrderedItems { OrderedItemdId = 2, OrderId = 1, ProductId = 1002, Quantity = 1, Price = 30 },
            new OrderedItems { OrderedItemdId = 3, OrderId = 2, ProductId = 1003, Quantity = 1, Price = 20 },
            new OrderedItems { OrderedItemdId = 4, OrderId = 3, ProductId = 1003, Quantity = 3, Price = 20 },
            new OrderedItems { OrderedItemdId = 5, OrderId = 3, ProductId = 1003, Quantity = 2, Price = 20 },
            new OrderedItems { OrderedItemdId = 6, OrderId = 3, ProductId = 1003, Quantity = 1, Price = 20 },
            new OrderedItems { OrderedItemdId = 7, OrderId = 3, ProductId = 1003, Quantity = 4, Price = 20 },
            new OrderedItems { OrderedItemdId = 8, OrderId = 3, ProductId = 1003, Quantity = 1, Price = 20 },
            new OrderedItems { OrderedItemdId = 9, OrderId = 3, ProductId = 1003, Quantity = 2, Price = 20 },
            new OrderedItems { OrderedItemdId = 10, OrderId = 3, ProductId = 1003, Quantity = 1, Price = 20 },
            new OrderedItems { OrderedItemdId = 11, OrderId = 1, ProductId = 1001, Quantity = 1, Price = 50 },
            new OrderedItems { OrderedItemdId = 12, OrderId = 2, ProductId = 1003, Quantity = 1, Price = 20 }
        };

var refunds = new List<Refund>
        {
            new Refund { OrderId = 1, CustomerId = 101 },
            new Refund { OrderId = 2, CustomerId = 102 },

        };

var paymentMethods = new List<PaymentMethods>
        {
            new PaymentMethods { PaymentMethodId = 1, PaymentType = "Kapıda" },
            new PaymentMethods { PaymentMethodId = 2, PaymentType = "Online" }
        };

var orderService = new OrderService(orders, orderedItems, paymentMethods);


// Kapıda ödeme olan siparişleri filtrele ve değerleri güncelle
orderService.UpdateOrderValues();

// 
var updatedRefund = orderService.checkExist(2, 101, refunds);
var updatedRefund2 = orderService.checkExist(1, 101, refunds);


public class Orders
{

    public int OrderId { get; set; }

    public int CustomerId { get; set; }

    public string? PaymentMethod { get; set; }


}

public class OrderedItems
{

    public int OrderedItemdId { get; set; }

    public int OrderId { get; set; } // Buradaki OrderId, yukarıdaki Orders class'ındaki foreign key'idir.

    public int ProductId { get; set; }

    public int Quantity { get; set; }

    public int Price { get; set; }


}



public class PaymentMethods
{

    public int PaymentMethodId { get; set; }

    public string? PaymentType { get; set; } // 1=Kapıda, 2=Online ödeme gibi
}

public class Refund
{

    public int OrderId { get; set; }

    public int CustomerId { get; set; }

}