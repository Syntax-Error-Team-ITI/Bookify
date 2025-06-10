namespace Bookify.Web.Repository
{
    public class GenericRepository<TEntity> where TEntity : BaseModel
    {
        protected ApplicationDbContext db;
        public GenericRepository(ApplicationDbContext _db)
        {
            db = _db;
        }


        public IEnumerable<TEntity> GetAll()
        {
            return db.Set<TEntity>().ToList();
        }
        public IEnumerable<TEntity> GetNotDeleted()
        {
            return db.Set<TEntity>().Where(e => !e.IsDeleted).ToList();
        }

        public TEntity? GetById(int id)
        {
            return db.Set<TEntity>().Find(id);
        }

        public void Add(TEntity entity)
        {
            db.Set<TEntity>().Add(entity);
        }

        public void Update(TEntity entity)
        {
            db.Set<TEntity>().Update(entity);
        }

        public void Delete(TEntity entity)
        {
            db.Set<TEntity>().Remove(entity);
        }

        public void Save()
        {
            db.SaveChanges();
        }
    }
}
