

//using Microsoft.AspNet.Identity;
//using System;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.Data.Entity;
//using System.Data.Entity.Infrastructure;
//using System.Linq;
//using System.Security.Claims;
//using System.Threading;
//using System.Threading.Tasks;
//using Ubik.Web.SSO.Contracts;

//namespace Ubik.Web.SSO
//{


 
//    /// <summary>
//    /// Creates a new instance of a persistence store for roles.
//    /// </summary>
//    /// <typeparam name="TRole">The type of the class representing a role</typeparam>
//    public class RoleStore<TRole> : RoleStore<TRole, AuthDbContext, int>, IRoleStoreWithCustomClaims
//        where TRole : UbikRole
//    {
//        public RoleStore(DbContext context, IdentityErrorDescriber describer = null) : base(context, describer) { }


//        public async Task<IEnumerable<Claim>> RoleRelatedClaims(int userId)
//        {
//            return
//             await Roles.Where(x => x.Users.Any(user => user.UserId == userId))
//                    .SelectMany(role => role.RoleClaims)
//                    .Distinct()
//                    .Select(appClaim => new Claim(appClaim.ClaimType, appClaim.Value))
//                    .ToListAsync();
//        }

//        public async Task<IdentityResult> ClearAllRoleClaims(string role, CancellationToken cancellationToken)
//        {
//            var db = Context as AuthDbContext;
//            var claims = from claim in db.RoleClaims
//                         join r in db.Roles on claim.RoleId equals r.Id
//                         where r.Name == role
//                         select claim;
//            foreach (var applicationClaim in claims)
//            {
//                db.RoleClaims.Remove(applicationClaim);
//            }
//            try
//            {
//                await db.SaveChangesAsync();
//                return await Task.FromResult(IdentityResult.Success);
//            }
//            catch (Exception ex)
//            {
//                return IdentityResult.Failed(new IdentityError() { Description = ex.Message });
//            }
//        }
//    }

//    /// <summary>
//    /// Creates a new instance of a persistence store for roles.
//    /// </summary>
//    /// <typeparam name="TRole">The type of the class representing a role.</typeparam>
//    /// <typeparam name="TContext">The type of the data context class used to access the store.</typeparam>
//    public class RoleStore<TRole, TContext> : RoleStore<TRole, TContext, int>
//        where TRole : IdentityRole<int>
//        where TContext : DbContext
//    {
//        public RoleStore(TContext context, IdentityErrorDescriber describer = null) : base(context, describer) { }
//    }

//    /// <summary>
//    /// Creates a new instance of a persistence store for roles.
//    /// </summary>
//    /// <typeparam name="TRole">The type of the class representing a role.</typeparam>
//    /// <typeparam name="TContext">The type of the data context class used to access the store.</typeparam>
//    /// <typeparam name="TKey">The type of the primary key for a role.</typeparam>
//    public class RoleStore<TRole, TContext, TKey> :
//        IQueryableRoleStore<TRole>,
//        IRoleClaimStore<TRole>
//        where TRole : IdentityRole<TKey>
//        where TKey : IEquatable<TKey>
//        where TContext : DbContext
//    {
//        public RoleStore(TContext context, IdentityErrorDescriber describer = null)
//        {
//            if (context == null)
//            {
//                throw new ArgumentNullException(nameof(context));
//            }
//            Context = context;
//            ErrorDescriber = describer ?? new IdentityErrorDescriber();
//        }

//        private bool _disposed;


//        /// <summary>
//        /// Gets the database context for this store.
//        /// </summary>
//        public TContext Context { get; private set; }

//        /// <summary>
//        /// Gets or sets the <see cref="IdentityErrorDescriber"/> for any error that occurred with the current operation.
//        /// </summary>
//        public IdentityErrorDescriber ErrorDescriber { get; set; }

//        /// <summary>
//        /// Gets or sets a flag indicating if changes should be persisted after CreateAsync, UpdateAsync and DeleteAsync are called.
//        /// </summary>
//        /// <value>
//        /// True if changes should be automatically persisted, otherwise false.
//        /// </value>
//        public bool AutoSaveChanges { get; set; } = true;

//        /// <summary>Saves the current store.</summary>
//        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
//        /// <returns>The <see cref="Task"/> that represents the asynchronous operation.</returns>
//        private async Task SaveChanges(CancellationToken cancellationToken)
//        {
//            if (AutoSaveChanges)
//            {
//                await Context.SaveChangesAsync(cancellationToken);
//            }
//        }

//        /// <summary>
//        /// Creates a new role in a store as an asynchronous operation.
//        /// </summary>
//        /// <param name="role">The role to create in the store.</param>
//        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
//        /// <returns>A <see cref="Task{TResult}"/> that represents the <see cref="IdentityResult"/> of the asynchronous query.</returns>
//        public async virtual Task<IdentityResult> CreateAsync(TRole role, CancellationToken cancellationToken = default(CancellationToken))
//        {
//            cancellationToken.ThrowIfCancellationRequested();
//            ThrowIfDisposed();
//            if (role == null)
//            {
//                throw new ArgumentNullException(nameof(role));
//            }
//            Context.Set<TRole>().Add(role);
//            await SaveChanges(cancellationToken);
//            return IdentityResult.Success;
//        }

//        /// <summary>
//        /// Updates a role in a store as an asynchronous operation.
//        /// </summary>
//        /// <param name="role">The role to update in the store.</param>
//        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
//        /// <returns>A <see cref="Task{TResult}"/> that represents the <see cref="IdentityResult"/> of the asynchronous query.</returns>
//        public async virtual Task<IdentityResult> UpdateAsync(TRole role, CancellationToken cancellationToken = default(CancellationToken))
//        {
//            cancellationToken.ThrowIfCancellationRequested();
//            ThrowIfDisposed();
//            if (role == null)
//            {
//                throw new ArgumentNullException(nameof(role));
//            }
//            Context.Set<TRole>().Attach(role);
//            role.ConcurrencyStamp = Guid.NewGuid().ToString();
//            Context.Entry<TRole>(role).State = EntityState.Modified;
//            try
//            {
//                await SaveChanges(cancellationToken);
//            }
//            catch (DbUpdateConcurrencyException)
//            {
//                return IdentityResult.Failed(ErrorDescriber.ConcurrencyFailure());
//            }
//            return IdentityResult.Success;
//        }

//        /// <summary>
//        /// Deletes a role from the store as an asynchronous operation.
//        /// </summary>
//        /// <param name="role">The role to delete from the store.</param>
//        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
//        /// <returns>A <see cref="Task{TResult}"/> that represents the <see cref="IdentityResult"/> of the asynchronous query.</returns>
//        public async virtual Task<IdentityResult> DeleteAsync(TRole role, CancellationToken cancellationToken = default(CancellationToken))
//        {
//            cancellationToken.ThrowIfCancellationRequested();
//            ThrowIfDisposed();
//            if (role == null)
//            {
//                throw new ArgumentNullException(nameof(role));
//            }
//            Context.Set<TRole>().Remove(role);
//            try
//            {
//                await SaveChanges(cancellationToken);
//            }
//            catch (DbUpdateConcurrencyException)
//            {
//                return IdentityResult.Failed(ErrorDescriber.ConcurrencyFailure());
//            }
//            return IdentityResult.Success;
//        }

//        /// <summary>
//        /// Gets the ID for a role from the store as an asynchronous operation.
//        /// </summary>
//        /// <param name="role">The role whose ID should be returned.</param>
//        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
//        /// <returns>A <see cref="Task{TResult}"/> that contains the ID of the role.</returns>
//        public Task<string> GetRoleIdAsync(TRole role, CancellationToken cancellationToken = default(CancellationToken))
//        {
//            cancellationToken.ThrowIfCancellationRequested();
//            ThrowIfDisposed();
//            if (role == null)
//            {
//                throw new ArgumentNullException(nameof(role));
//            }
//            return Task.FromResult(ConvertIdToString(role.Id));
//        }

//        /// <summary>
//        /// Gets the name of a role from the store as an asynchronous operation.
//        /// </summary>
//        /// <param name="role">The role whose name should be returned.</param>
//        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
//        /// <returns>A <see cref="Task{TResult}"/> that contains the name of the role.</returns>
//        public Task<string> GetRoleNameAsync(TRole role, CancellationToken cancellationToken = default(CancellationToken))
//        {
//            cancellationToken.ThrowIfCancellationRequested();
//            ThrowIfDisposed();
//            if (role == null)
//            {
//                throw new ArgumentNullException(nameof(role));
//            }
//            return Task.FromResult(role.Name);
//        }

//        /// <summary>
//        /// Sets the name of a role in the store as an asynchronous operation.
//        /// </summary>
//        /// <param name="role">The role whose name should be set.</param>
//        /// <param name="roleName">The name of the role.</param>
//        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
//        /// <returns>The <see cref="Task"/> that represents the asynchronous operation.</returns>
//        public Task SetRoleNameAsync(TRole role, string roleName, CancellationToken cancellationToken = default(CancellationToken))
//        {
//            cancellationToken.ThrowIfCancellationRequested();
//            ThrowIfDisposed();
//            if (role == null)
//            {
//                throw new ArgumentNullException(nameof(role));
//            }
//            role.Name = roleName;
//            return Task.FromResult(0);
//        }

//        /// <summary>
//        /// Converts the provided <paramref name="id"/> to a strongly typed key object.
//        /// </summary>
//        /// <param name="id">The id to convert.</param>
//        /// <returns>An instance of <typeparamref name="TKey"/> representing the provided <paramref name="id"/>.</returns>
//        public virtual TKey ConvertIdFromString(string id)
//        {
//            if (id == null)
//            {
//                return default(TKey);
//            }
//            return (TKey)TypeDescriptor.GetConverter(typeof(TKey)).ConvertFromInvariantString(id);
//        }

//        /// <summary>
//        /// Converts the provided <paramref name="id"/> to its string representation.
//        /// </summary>
//        /// <param name="id">The id to convert.</param>
//        /// <returns>An <see cref="string"/> representation of the provided <paramref name="id"/>.</returns>
//        public virtual string ConvertIdToString(TKey id)
//        {
//            if (id.Equals(default(TKey)))
//            {
//                return null;
//            }
//            return id.ToString();
//        }

//        /// <summary>
//        /// Finds the role who has the specified ID as an asynchronous operation.
//        /// </summary>
//        /// <param name="roleId">The role ID to look for.</param>
//        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
//        /// <returns>A <see cref="Task{TResult}"/> that result of the look up.</returns>
//        public virtual Task<TRole> FindByIdAsync(string id, CancellationToken cancellationToken = default(CancellationToken))
//        {
//            cancellationToken.ThrowIfCancellationRequested();
//            ThrowIfDisposed();
//            var roleId = ConvertIdFromString(id);
//            return Roles.FirstOrDefaultAsync(u => u.Id.Equals(roleId), cancellationToken);
//        }

//        /// <summary>
//        /// Finds the role who has the specified normalized name as an asynchronous operation.
//        /// </summary>
//        /// <param name="normalizedRoleName">The normalized role name to look for.</param>
//        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
//        /// <returns>A <see cref="Task{TResult}"/> that result of the look up.</returns>
//        public virtual Task<TRole> FindByNameAsync(string normalizedName, CancellationToken cancellationToken = default(CancellationToken))
//        {
//            cancellationToken.ThrowIfCancellationRequested();
//            ThrowIfDisposed();
//            return Roles.FirstOrDefaultAsync(r => r.NormalizedName == normalizedName, cancellationToken);
//        }

//        /// <summary>
//        /// Get a role's normalized name as an asynchronous operation.
//        /// </summary>
//        /// <param name="role">The role whose normalized name should be retrieved.</param>
//        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
//        /// <returns>A <see cref="Task{TResult}"/> that contains the name of the role.</returns>
//        public virtual Task<string> GetNormalizedRoleNameAsync(TRole role, CancellationToken cancellationToken = default(CancellationToken))
//        {
//            cancellationToken.ThrowIfCancellationRequested();
//            ThrowIfDisposed();
//            if (role == null)
//            {
//                throw new ArgumentNullException(nameof(role));
//            }
//            return Task.FromResult(role.NormalizedName);
//        }

//        /// <summary>
//        /// Set a role's normalized name as an asynchronous operation.
//        /// </summary>
//        /// <param name="role">The role whose normalized name should be set.</param>
//        /// <param name="normalizedName">The normalized name to set</param>
//        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
//        /// <returns>The <see cref="Task"/> that represents the asynchronous operation.</returns>
//        public virtual Task SetNormalizedRoleNameAsync(TRole role, string normalizedName, CancellationToken cancellationToken = default(CancellationToken))
//        {
//            cancellationToken.ThrowIfCancellationRequested();
//            ThrowIfDisposed();
//            if (role == null)
//            {
//                throw new ArgumentNullException(nameof(role));
//            }
//            role.NormalizedName = normalizedName;
//            return Task.FromResult(0);
//        }

//        private void ThrowIfDisposed()
//        {
//            if (_disposed)
//            {
//                throw new ObjectDisposedException(GetType().Name);
//            }
//        }

//        /// <summary>
//        /// Dispose the stores
//        /// </summary>
//        public void Dispose()
//        {
//            _disposed = true;
//        }

//        /// <summary>
//        /// Get the claims associated with the specified <paramref name="role"/> as an asynchronous operation.
//        /// </summary>
//        /// <param name="role">The role whose claims should be retrieved.</param>
//        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
//        /// <returns>A <see cref="Task{TResult}"/> that contains the claims granted to a role.</returns>
//        public async Task<IList<Claim>> GetClaimsAsync(TRole role, CancellationToken cancellationToken = default(CancellationToken))
//        {
//            ThrowIfDisposed();
//            if (role == null)
//            {
//                throw new ArgumentNullException(nameof(role));
//            }

//            return await RoleClaims.Where(rc => rc.RoleId.Equals(role.Id)).Select(c => new Claim(c.ClaimType, c.ClaimValue)).ToListAsync(cancellationToken);
//        }

//        /// <summary>
//        /// Adds the <paramref name="claim"/> given to the specified <paramref name="role"/>.
//        /// </summary>
//        /// <param name="role">The role to add the claim to.</param>
//        /// <param name="claim">The claim to add to the role.</param>
//        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
//        /// <returns>The <see cref="Task"/> that represents the asynchronous operation.</returns>
//        public Task AddClaimAsync(TRole role, Claim claim, CancellationToken cancellationToken = default(CancellationToken))
//        {
//            ThrowIfDisposed();
//            if (role == null)
//            {
//                throw new ArgumentNullException(nameof(role));
//            }
//            if (claim == null)
//            {
//                throw new ArgumentNullException(nameof(claim));
//            }

//            RoleClaims.Add(new IdentityRoleClaim<TKey> { RoleId = role.Id, ClaimType = claim.Type, ClaimValue = claim.Value });

//            return Task.FromResult(false);
//        }

//        /// <summary>
//        /// Removes the <paramref name="claim"/> given from the specified <paramref name="role"/>.
//        /// </summary>
//        /// <param name="role">The role to remove the claim from.</param>
//        /// <param name="claim">The claim to remove from the role.</param>
//        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
//        /// <returns>The <see cref="Task"/> that represents the asynchronous operation.</returns>
//        public async Task RemoveClaimAsync(TRole role, Claim claim, CancellationToken cancellationToken = default(CancellationToken))
//        {
//            ThrowIfDisposed();
//            if (role == null)
//            {
//                throw new ArgumentNullException(nameof(role));
//            }
//            if (claim == null)
//            {
//                throw new ArgumentNullException(nameof(claim));
//            }
//            var claims = await RoleClaims.Where(rc => rc.RoleId.Equals(role.Id) && rc.ClaimValue == claim.Value && rc.ClaimType == claim.Type).ToListAsync(cancellationToken);
//            foreach (var c in claims)
//            {
//                RoleClaims.Remove(c);
//            }
//        }

//        /// <summary>
//        /// A navigation property for the roles the store contains.
//        /// </summary>
//        public virtual IQueryable<TRole> Roles
//        {
//            get { return Context.Set<TRole>(); }
//        }

//        private DbSet<IdentityRoleClaim<TKey>> RoleClaims { get { return Context.Set<IdentityRoleClaim<TKey>>(); } }
//    }
//}
