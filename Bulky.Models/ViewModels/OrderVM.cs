namespace BulkyBook.Models.ViewModels
{
    public class OrderVM
	{
		public OrderHeaderModel OrderHeader { get; set; }
		public IEnumerable<OrderDetailModel> OrderDetail { get; set; }
	}
}
