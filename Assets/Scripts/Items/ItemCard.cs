namespace Items
{
    public class ItemCard : Item
    {
        public ElementType ElType { get; private set; }
        public int Rank { get; private set; }
        
        public override void Initialize(ElementType elementType, int rank)
        {
            ElType = elementType;
            Rank = rank;
            
            ItemName = $"{ItemName} {ElType} {Rank}";
        }

        public override void Use()
        {
            throw new System.NotImplementedException();
        }
    }
}
