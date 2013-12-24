using System;
using OBEHR.Models;
using OBEHR.Models.Base;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Web;
using System.Linq;
using System.Linq.Expressions;

namespace OBEHR.Models.DAL
{
    public class UnitOfWork : IDisposable
    {
        public OBEHRContext context = new OBEHRContext();
        private GenericRepository<PPUser> ppUserRepository;
        private GenericRepository<AccumulationType> accumulationTypeRepository;
        private GenericRepository<PensionType> pensionTypeRepository;
        private GenericRepository<Assurance> assuranceRepository;
        private GenericRepository<BudgetCenter> budgetCenterRepository;
        private GenericRepository<ContractType> contractTypeRepository;
        private GenericRepository<Department> departmentRepository;
        private GenericRepository<Level> levelRepository;
        private GenericRepository<Position> positionRepository;
        private GenericRepository<Zhangtao> zhangtaoRepository;
        private GenericRepository<Certificate> certificateRepository;
        private GenericRepository<City> cityRepository;
        private GenericRepository<Client> clientRepository;
        private GenericRepository<Supplier> supplierRepository;
        private GenericRepository<Document> documentRepository;
        private GenericRepository<ClientCity> clientCityRepository;
        private GenericRepository<AccumulationRule> accumulationRuleRepository;
        private GenericRepository<PensionRule> pensionRuleRepository;

        public GenericRepository<AccumulationType> AccumulationTypeRepository
        {
            get
            {

                if (this.accumulationTypeRepository == null)
                {
                    this.accumulationTypeRepository = new GenericRepository<AccumulationType>(context);
                }
                return accumulationTypeRepository;
            }
        }
        public GenericRepository<PensionType> PensionTypeRepository
        {
            get
            {

                if (this.pensionTypeRepository == null)
                {
                    this.pensionTypeRepository = new GenericRepository<PensionType>(context);
                }
                return pensionTypeRepository;
            }
        }
        public GenericRepository<Assurance> AssuranceRepository
        {
            get
            {

                if (this.assuranceRepository == null)
                {
                    this.assuranceRepository = new GenericRepository<Assurance>(context);
                }
                return assuranceRepository;
            }
        }
        public GenericRepository<BudgetCenter> BudgetCenterRepository
        {
            get
            {

                if (this.budgetCenterRepository == null)
                {
                    this.budgetCenterRepository = new GenericRepository<BudgetCenter>(context);
                }
                return budgetCenterRepository;
            }
        }
        public GenericRepository<ContractType> ContractTypeRepository
        {
            get
            {

                if (this.contractTypeRepository == null)
                {
                    this.contractTypeRepository = new GenericRepository<ContractType>(context);
                }
                return contractTypeRepository;
            }
        }
        public GenericRepository<Department> DepartmentRepository
        {
            get
            {

                if (this.departmentRepository == null)
                {
                    this.departmentRepository = new GenericRepository<Department>(context);
                }
                return departmentRepository;
            }
        }
        public GenericRepository<Level> LevelRepository
        {
            get
            {

                if (this.levelRepository == null)
                {
                    this.levelRepository = new GenericRepository<Level>(context);
                }
                return levelRepository;
            }
        }
        public GenericRepository<Position> PositionRepository
        {
            get
            {

                if (this.positionRepository == null)
                {
                    this.positionRepository = new GenericRepository<Position>(context);
                }
                return positionRepository;
            }
        }
        public GenericRepository<Zhangtao> ZhangtaoRepository
        {
            get
            {

                if (this.zhangtaoRepository == null)
                {
                    this.zhangtaoRepository = new GenericRepository<Zhangtao>(context);
                }
                return zhangtaoRepository;
            }
        }
        public GenericRepository<PPUser> PPUserRepository
        {
            get
            {

                if (this.ppUserRepository == null)
                {
                    this.ppUserRepository = new GenericRepository<PPUser>(context);
                }
                return ppUserRepository;
            }
        }

        public GenericRepository<Client> ClientRepository
        {
            get
            {

                if (this.clientRepository == null)
                {
                    this.clientRepository = new GenericRepository<Client>(context);
                }
                return clientRepository;
            }
        }

        public GenericRepository<Certificate> CertificateRepository
        {
            get
            {

                if (this.certificateRepository == null)
                {
                    this.certificateRepository = new GenericRepository<Certificate>(context);
                }
                return certificateRepository;
            }
        }

        public GenericRepository<City> CityRepository
        {
            get
            {

                if (this.cityRepository == null)
                {
                    this.cityRepository = new GenericRepository<City>(context);
                }
                return cityRepository;
            }
        }

        public GenericRepository<Supplier> SupplierRepository
        {
            get
            {

                if (this.supplierRepository == null)
                {
                    this.supplierRepository = new GenericRepository<Supplier>(context);
                }
                return supplierRepository;
            }
        }

        public GenericRepository<Document> DocumentRepository
        {
            get
            {

                if (this.documentRepository == null)
                {
                    this.documentRepository = new GenericRepository<Document>(context);
                }
                return documentRepository;
            }
        }

        public GenericRepository<ClientCity> ClientCityRepository
        {
            get
            {

                if (this.clientCityRepository == null)
                {
                    this.clientCityRepository = new GenericRepository<ClientCity>(context);
                }
                return clientCityRepository;
            }
        }
        public GenericRepository<AccumulationRule> AccumulationRuleRepository
        {
            get
            {

                if (this.accumulationRuleRepository == null)
                {
                    this.accumulationRuleRepository = new GenericRepository<AccumulationRule>(context);
                }
                return accumulationRuleRepository;
            }
        }
        public GenericRepository<PensionRule> PensionRuleRepository
        {
            get
            {

                if (this.pensionRuleRepository == null)
                {
                    this.pensionRuleRepository = new GenericRepository<PensionRule>(context);
                }
                return pensionRuleRepository;
            }
        }

        public void PPSave(bool admin = false)
        {
            //Do soft deletes
            foreach (var deletableEntity in context.ChangeTracker.Entries<BaseModel>())
            {
                if (deletableEntity.State == EntityState.Deleted)
                {
                    //Deleted - set the deleted flag
                    deletableEntity.State = EntityState.Unchanged; //We need to set this to unchanged here, because setting it to modified seems to set ALL of its fields to modified
                    deletableEntity.Entity.IsDeleted = true; //This will set the entity's state to modified for the next time we query the ChangeTracker
                }
            }
            if (!admin)
            {

                var userId = HttpContext.Current.User.Identity.GetUserId();
                context.Logger.SaveChanges(context.UserManager.FindById(userId), SaveOptions.AcceptAllChangesAfterSave);
            }
            else
            {
                //默认使用admin记录log
                var ppUser = context.UserManager.FindByName<PPUser>("Admin");
                context.Logger.SaveChanges(ppUser, SaveOptions.AcceptAllChangesAfterSave);
            }
        }


        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}