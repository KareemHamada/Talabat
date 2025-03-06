

namespace Talabat.Repository.Data.Configurations
{
    public class OrderConfig : IEntityTypeConfiguration<Core.Entities.Order_Aggregate.Order>
    {
        public void Configure(EntityTypeBuilder<Core.Entities.Order_Aggregate.Order> builder)
        {
            builder.Property(O => O.Status)
                .HasConversion(OStatus => OStatus.ToString(), OStatus => (OrderStatus)Enum.Parse(typeof(OrderStatus), OStatus));

            builder.Property(O => O.SubTotal).HasColumnType("decimal(18,2)");

            builder.OwnsOne(O => O.ShippingAddress, X => X.WithOwner());

            builder.HasOne(O=>O.DeliveryMethod)
                .WithMany()
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
