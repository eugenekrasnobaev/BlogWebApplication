namespace BlogDAL.Interfaces
{
    public interface IEntity
    {
        int Id { get; }

        bool IsActive { get; set; }
    }
}
