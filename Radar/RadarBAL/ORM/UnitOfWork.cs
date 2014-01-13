using RadarModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core;
//using System.Data.Entity.Core;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RadarBAL.ORM
{
    public class UnitOfWork : IDisposable
    {
        private RadarContext context = new RadarContext();
        private GenericRepository<Category> _categoryRepository;
        private GenericRepository<Comment> _commentRepository;
        private GenericRepository<Company> _companyRepository;
        private GenericRepository<Employee> _employeeRepository;
        private GenericRepository<Location> _locationRepository;
        private GenericRepository<Message> _messageRepository;
        private GenericRepository<Notification> _notificationRepository;
        private GenericRepository<Post> _postRepository;
        private GenericRepository<Rating> _ratingRepository;
        private GenericRepository<Role> _roleRepository;
        private GenericRepository<User> _userRepository;

        public GenericRepository<Category> CategoryRepository
        {
            get
            {
                if (this._categoryRepository == null)
                {
                    this._categoryRepository = new GenericRepository<Category>(context);
                }
                return _categoryRepository;
            }
        }

        public GenericRepository<Comment> CommentRepository
        {
            get
            {
                if (this._commentRepository == null)
                {
                    this._commentRepository = new GenericRepository<Comment>(context);
                }
                return _commentRepository;
            }
        }

        public GenericRepository<Company> CompanyRepository
        {
            get
            {
                if (this._companyRepository == null)
                {
                    this._companyRepository = new GenericRepository<Company>(context);
                }
                return _companyRepository;
            }
        }

        public GenericRepository<Employee> EmployeeRepository
        {
            get
            {
                if (this._employeeRepository == null)
                {
                    this._employeeRepository = new GenericRepository<Employee>(context);
                }
                return _employeeRepository;
            }
        }

        public GenericRepository<Location> LocationRepository
        {
            get
            {
                if (this._locationRepository == null)
                {
                    this._locationRepository = new GenericRepository<Location>(context);
                }
                return _locationRepository;
            }
        }

        public GenericRepository<Message> MessageRepository
        {
            get
            {
                if (this._messageRepository == null)
                {
                    this._messageRepository = new GenericRepository<Message>(context);
                }
                return _messageRepository;
            }
        }

        public GenericRepository<Notification> NotificationRepository
        {
            get
            {
                if (this._notificationRepository == null)
                {
                    this._notificationRepository = new GenericRepository<Notification>(context);
                }
                return _notificationRepository;
            }
        }

        public GenericRepository<Post> PostRepository
        {
            get
            {
                if (this._postRepository == null)
                {
                    this._postRepository = new GenericRepository<Post>(context);
                }
                return _postRepository;
            }
        }

        public GenericRepository<Rating> RatingRepository
        {
            get
            {
                if (this._ratingRepository == null)
                {
                    this._ratingRepository = new GenericRepository<Rating>(context);
                }
                return _ratingRepository;
            }
        }

        public GenericRepository<Role> RoleRepository
        {
            get
            {
                if (this._roleRepository == null)
                {
                    this._roleRepository = new GenericRepository<Role>(context);
                }
                return _roleRepository;
            }
        }

        public GenericRepository<User> UserRepository
        {
            get
            {

                if (this._userRepository == null)
                {
                    this._userRepository = new GenericRepository<User>(context);
                }
                return _userRepository;
            }
        }

        public void Save()
        {
            try
            {
                context.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                var m = ex.EntityValidationErrors;
                var l = 3;
                l++;
            }
            catch (DbUpdateException ex)
            {
                var m = ex.InnerException;
                var l = 3;
                l++;
            }
            catch (OptimisticConcurrencyException ex)
            {
                var m = ex.InnerException;
                var l = 3;
                l++;
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
