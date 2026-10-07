namespace API_PI_ADM_Clubes.Model
{
    public class Sport : BaseEntity
    {
        public string Name { get; set; }

        public virtual ICollection<CourtSport> CourtSports { get; set; }
    }
}