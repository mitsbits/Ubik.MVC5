﻿//using Mehdime.Entity;
//using Microsoft.AspNet.Identity;
//using System;
//using System.Collections.Generic;
//using System.Data.Entity.Infrastructure;
//using System.Linq;
//using System.Security.Claims;
//using System.Threading;
//using System.Threading.Tasks;
//using Ubik.Web.SSO.Contracts;

//namespace Ubik.Web.SSO.Stores
//{
//    public class RoleStore :
//        IQueryableRoleStore<UbikRole>,
//        IRoleClaimStore<UbikRole>,
//        IRoleStoreWithCustomClaims

//    {
//        private readonly IDbContextScopeFactory _dbContextScopeFactory;
//        private readonly IUserRepository _userRepo;
//        private readonly IRoleRepository _roleRepo;
//        private readonly IClaimRepository _claimRepo;

//        public RoleStore(IDbContextScopeFactory dbContextScopeFactory, IUserRepository userRepo, IRoleRepository roleRepo, IClaimRepository claimRepo, IdentityErrorDescriber describer = null)
//        {
//            _dbContextScopeFactory = dbContextScopeFactory;
//            _userRepo = userRepo;
//            _roleRepo = roleRepo;
//            _claimRepo = claimRepo;
//            ErrorDescriber = describer ?? new IdentityErrorDescriber();
//        }

//        private bool _disposed;

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

//        /// <summary>
//        /// Creates a new role in a store as an asynchronous operation.
//        /// </summary>
//        /// <param name="role">The role to create in the store.</param>
//        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
//        /// <returns>A <see cref="Task{TResult}"/> that represents the <see cref="IdentityResult"/> of the asynchronous query.</returns>
//        public async virtual Task<IdentityResult> CreateAsync(UbikRole role, CancellationToken cancellationToken = default(CancellationToken))
//        {
//            cancellationToken.ThrowIfCancellationRequested();
//            ThrowIfDisposed();
//            if (role == null)
//            {
//                throw new ArgumentNullException(nameof(role));
//            }

//            using (var db = _dbContextScopeFactory.CreateWithTransaction(System.Data.IsolationLevel.ReadCommitted))
//            {
//                try
//                {
//                    await _roleRepo.CreateAsync(role);
//                    await db.SaveChangesAsync(cancellationToken);
//                }
//                catch (DbUpdateConcurrencyException)
//                {
//                    return IdentityResult.Failed(ErrorDescriber.ConcurrencyFailure());
//                }
//            }
//            return IdentityResult.Success;
//        }

//        /// <summary>
//        /// Updates a role in a store as an asynchronous operation.
//        /// </summary>
//        /// <param name="role">The role to update in the store.</param>
//        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
//        /// <returns>A <see cref="Task{TResult}"/> that represents the <see cref="IdentityResult"/> of the asynchronous query.</returns>
//        public async virtual Task<IdentityResult> UpdateAsync(UbikRole role, CancellationToken cancellationToken = default(CancellationToken))
//        {
//            cancellationToken.ThrowIfCancellationRequested();
//            ThrowIfDisposed();
//            if (role == null)
//            {
//                throw new ArgumentNullException(nameof(role));
//            }
//            using (var db = _dbContextScopeFactory.CreateWithTransaction(System.Data.IsolationLevel.ReadCommitted))
//            {
//                try
//                {
//                    await _roleRepo.UpdateAsync(role);
//                    await db.SaveChangesAsync(cancellationToken);
//                }
//                catch (DbUpdateConcurrencyException)
//                {
//                    return IdentityResult.Failed(ErrorDescriber.ConcurrencyFailure());
//                }
//            }

//            return IdentityResult.Success;
//        }

//        /// <summary>
//        /// Deletes a role from the store as an asynchronous operation.
//        /// </summary>
//        /// <param name="role">The role to delete from the store.</param>
//        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
//        /// <returns>A <see cref="Task{TResult}"/> that represents the <see cref="IdentityResult"/> of the asynchronous query.</returns>
//        public async virtual Task<IdentityResult> DeleteAsync(UbikRole role, CancellationToken cancellationToken = default(CancellationToken))
//        {
//            cancellationToken.ThrowIfCancellationRequested();
//            ThrowIfDisposed();
//            if (role == null)
//            {
//                throw new ArgumentNullException(nameof(role));
//            }
//            using (var db = _dbContextScopeFactory.CreateWithTransaction(System.Data.IsolationLevel.ReadCommitted))
//            {
//                try
//                {
//                    await _roleRepo.DeleteAsync(x => x.Id == role.Id);
//                    await db.SaveChangesAsync(cancellationToken);
//                }
//                catch (DbUpdateConcurrencyException)
//                {
//                    return IdentityResult.Failed(ErrorDescriber.ConcurrencyFailure());
//                }
//            }

//            return IdentityResult.Success;
//        }

//        /// <summary>
//        /// Gets the ID for a role from the store as an asynchronous operation.
//        /// </summary>
//        /// <param name="role">The role whose ID should be returned.</param>
//        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
//        /// <returns>A <see cref="Task{TResult}"/> that contains the ID of the role.</returns>
//        public Task<string> GetRoleIdAsync(UbikRole role, CancellationToken cancellationToken = default(CancellationToken))
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
//        public Task<string> GetRoleNameAsync(UbikRole role, CancellationToken cancellationToken = default(CancellationToken))
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
//        public Task SetRoleNameAsync(UbikRole role, string roleName, CancellationToken cancellationToken = default(CancellationToken))
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
//        public virtual int ConvertIdFromString(string id)
//        {
//            if (id == null)
//            {
//                return default(int);
//            }
//            return int.Parse(id);
//        }

//        /// <summary>
//        /// Converts the provided <paramref name="id"/> to its string representation.
//        /// </summary>
//        /// <param name="id">The id to convert.</param>
//        /// <returns>An <see cref="string"/> representation of the provided <paramref name="id"/>.</returns>
//        public virtual string ConvertIdToString(int id)
//        {
//            if (id.Equals(default(int)))
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
//        public async virtual Task<UbikRole> FindByIdAsync(string id, CancellationToken cancellationToken = default(CancellationToken))
//        {
//            cancellationToken.ThrowIfCancellationRequested();
//            ThrowIfDisposed();
//            var roleId = ConvertIdFromString(id);
//            using (_dbContextScopeFactory.CreateReadOnly())
//            {
//                return await _roleRepo.GetAsync(x => x.Id == roleId);
//            }
//            //return Roles.FirstOrDefaultAsync(u => u.Id.Equals(roleId), cancellationToken);
//        }

//        /// <summary>
//        /// Finds the role who has the specified normalized name as an asynchronous operation.
//        /// </summary>
//        /// <param name="normalizedRoleName">The normalized role name to look for.</param>
//        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
//        /// <returns>A <see cref="Task{TResult}"/> that result of the look up.</returns>
//        public async virtual Task<UbikRole> FindByNameAsync(string normalizedName, CancellationToken cancellationToken = default(CancellationToken))
//        {
//            cancellationToken.ThrowIfCancellationRequested();
//            ThrowIfDisposed();
//            using (_dbContextScopeFactory.CreateReadOnly())
//            {
//                return await _roleRepo.GetAsync(x => x.NormalizedName == normalizedName);
//            }
//            //return Roles.FirstOrDefaultAsync(r => r.NormalizedName == normalizedName, cancellationToken);
//        }

//        /// <summary>
//        /// Get a role's normalized name as an asynchronous operation.
//        /// </summary>
//        /// <param name="role">The role whose normalized name should be retrieved.</param>
//        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
//        /// <returns>A <see cref="Task{TResult}"/> that contains the name of the role.</returns>
//        public virtual Task<string> GetNormalizedRoleNameAsync(UbikRole role, CancellationToken cancellationToken = default(CancellationToken))
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
//        public virtual Task SetNormalizedRoleNameAsync(UbikRole role, string normalizedName, CancellationToken cancellationToken = default(CancellationToken))
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
//        public async Task<IList<Claim>> GetClaimsAsync(UbikRole role, CancellationToken cancellationToken = default(CancellationToken))
//        {
//            ThrowIfDisposed();
//            if (role == null)
//            {
//                throw new ArgumentNullException(nameof(role));
//            }
//            using (_dbContextScopeFactory.CreateReadOnly())
//            {
//                var claims = await _claimRepo.FindAsync(x => x.Role.Id == role.Id, null, 1, 1000);
//                return claims.Select(c => new Claim(c.ClaimType, c.Value)).ToList();
//            }

//            //return await RoleClaims.Where(rc => rc.RoleId.Equals(role.Id)).Select(c => new Claim(c.ClaimType, c.ClaimValue)).ToListAsync(cancellationToken);
//        }

//        /// <summary>
//        /// Adds the <paramref name="claim"/> given to the specified <paramref name="role"/>.
//        /// </summary>
//        /// <param name="role">The role to add the claim to.</param>
//        /// <param name="claim">The claim to add to the role.</param>
//        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
//        /// <returns>The <see cref="Task"/> that represents the asynchronous operation.</returns>
//        public async Task AddClaimAsync(UbikRole role, Claim claim, CancellationToken cancellationToken = default(CancellationToken))
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
//            using (var db = _dbContextScopeFactory.CreateWithTransaction(System.Data.IsolationLevel.ReadCommitted))
//            {
//                var exists = await _claimRepo.GetAsync(x => x.ApplicationRoleId == role.Id.ToString() && x.ClaimType == claim.Type);
//                if (exists != null)
//                {
//                    exists.Value = claim.Value;
//                }
//                else
//                {
//                    await _claimRepo.CreateAsync(new ApplicationClaim(claim) { ApplicationRoleId = ConvertIdToString(role.Id) });
//                }
//                await db.SaveChangesAsync(cancellationToken);
//            }

//            //RoleClaims.Add(new IdentityRoleClaim<TKey> { RoleId = role.Id, ClaimType = claim.Type, ClaimValue = claim.Value });

//            //return Task.CompletedTask;
//        }

//        /// <summary>
//        /// Removes the <paramref name="claim"/> given from the specified <paramref name="role"/>.
//        /// </summary>
//        /// <param name="role">The role to remove the claim from.</param>
//        /// <param name="claim">The claim to remove from the role.</param>
//        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
//        /// <returns>The <see cref="Task"/> that represents the asynchronous operation.</returns>
//        public async Task RemoveClaimAsync(UbikRole role, Claim claim, CancellationToken cancellationToken = default(CancellationToken))
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
//            using (var db = _dbContextScopeFactory.CreateWithTransaction(System.Data.IsolationLevel.ReadCommitted))
//            {
//                await _claimRepo.DeleteAsync(x => x.ApplicationRoleId == role.Id.ToString() && x.ClaimType == claim.Type);
//                await db.SaveChangesAsync(cancellationToken);
//            }
//        }

//        public async Task<IdentityResult> ClearAllRoleClaims(string role, CancellationToken cancellationToken = default(CancellationToken))
//        {
//            ThrowIfDisposed();
//            if (string.IsNullOrWhiteSpace(role))
//            {
//                throw new ArgumentNullException(nameof(role));
//            }
//            using (var db = _dbContextScopeFactory.CreateWithTransaction(System.Data.IsolationLevel.ReadCommitted))
//            {
//                await _claimRepo.DeleteAsync(x => x.Role.Name == role);
//                await db.SaveChangesAsync(cancellationToken);
//            }

//            return IdentityResult.Success;
//        }

//        /// <summary>
//        /// A navigation property for the roles the store contains.
//        /// </summary>
//        public virtual IQueryable<UbikRole> Roles
//        {
//            get
//            {
//                return _roleRepo.GetQuery();
//            }
//        }
//    }
//}

using Mehdime.Entity;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Ubik.Web.SSO.Contracts;

namespace Ubik.Web.SSO.Stores
{
    public class RoleStore : RoleStoreBase<UbikRole, AuthDbContext, int>, IRoleStoreWithCustomClaims
    {
        public RoleStore(IAmbientDbContextLocator ambientDbContextLocator,
            IDbContextScopeFactory dbContextScopeFactory,
            IdentityErrorDescriber describer = null)
            : base(ambientDbContextLocator, dbContextScopeFactory, describer)
        { }

        public async Task<IdentityResult> ClearAllRoleClaims(string role, CancellationToken cancellationToken)
        {
            var entity = await Roles.FirstOrDefaultAsync(x => x.Name.Equals(role, StringComparison.InvariantCultureIgnoreCase), cancellationToken);
            if (entity != null)
            {
                entity.Claims.Clear();
                await SaveChanges(cancellationToken);
                return IdentityResult.Success;
            }
            return IdentityResult.Failed();
        }
    }

    public abstract class RoleStoreBase<TRole, TContext, TKey> :
        IQueryableRoleStore<TRole>,
        IRoleClaimStore<TRole>
        where TRole : IdentityRole<TKey>
        where TKey : IEquatable<TKey>
        where TContext : DbContext
    {
        public RoleStoreBase(IAmbientDbContextLocator ambientDbContextLocator,
            IDbContextScopeFactory dbContextScopeFactory, IdentityErrorDescriber describer = null)
        {
            _ambientDbContextLocator = ambientDbContextLocator;
            _dbContextScopeFactory = dbContextScopeFactory;
            ErrorDescriber = describer ?? new IdentityErrorDescriber();
            _db = _dbContextScopeFactory.Create();
        }

        private bool _disposed;

        private readonly IDbContextScopeFactory _dbContextScopeFactory;
        private readonly IAmbientDbContextLocator _ambientDbContextLocator;
        private readonly IDbContextScope _db;

        public TContext Context
        {
            get
            {
                var dbContext = _ambientDbContextLocator.Get<TContext>();

                if (dbContext == null)
                    throw new InvalidOperationException(
                        string.Format(@"No ambient DbContext of type {0} found.
                                        This means that this repository method has been called outside of the scope of a DbContextScope.
                                        A repository must only be accessed within the scope of a DbContextScope,
                                        which takes care of creating the DbContext instances that the repositories need and making them available as ambient contexts.
                                        This is what ensures that, for any given DbContext-derived type, the same instance is used throughout the duration of a business transaction.
                                        To fix this issue, use IDbContextScopeFactory in your top-level business logic service method to create a DbContextScope that wraps the entire business transaction
                                        that your service method implements.
                                        Then access this repository within that scope.
                                        Refer to the comments in the IDbContextScope.cs file for more details.",
                            typeof(TContext).Name));

                return dbContext;
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="IdentityErrorDescriber"/> for any error that occurred with the current operation.
        /// </summary>
        public IdentityErrorDescriber ErrorDescriber { get; set; }

        /// <summary>
        /// Gets or sets a flag indicating if changes should be persisted after CreateAsync, UpdateAsync and DeleteAsync are called.
        /// </summary>
        /// <value>
        /// True if changes should be automatically persisted, otherwise false.
        /// </value>
        public bool AutoSaveChanges { get; set; } = true;

        /// <summary>Saves the current store.</summary>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation.</returns>
        protected async Task SaveChanges(CancellationToken cancellationToken)
        {
            if (AutoSaveChanges)
            {
                // await Context.SaveChangesAsync(cancellationToken);
                await _db.SaveChangesAsync(cancellationToken);
            }
        }

        /// <summary>
        /// Creates a new role in a store as an asynchronous operation.
        /// </summary>
        /// <param name="role">The role to create in the store.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="Task{TResult}"/> that represents the <see cref="IdentityResult"/> of the asynchronous query.</returns>
        public async virtual Task<IdentityResult> CreateAsync(TRole role, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (role == null)
            {
                throw new ArgumentNullException(nameof(role));
            }
            Context.Set<TRole>().Add(role);
            await SaveChanges(cancellationToken);
            return IdentityResult.Success;
        }

        /// <summary>
        /// Updates a role in a store as an asynchronous operation.
        /// </summary>
        /// <param name="role">The role to update in the store.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="Task{TResult}"/> that represents the <see cref="IdentityResult"/> of the asynchronous query.</returns>
        public async virtual Task<IdentityResult> UpdateAsync(TRole role, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (role == null)
            {
                throw new ArgumentNullException(nameof(role));
            }
            Context.Set<TRole>().Attach(role);
            role.ConcurrencyStamp = Guid.NewGuid().ToString();
            Context.Entry<TRole>(role).State = EntityState.Modified;
            try
            {
                await SaveChanges(cancellationToken);
            }
            catch (DbUpdateConcurrencyException)
            {
                return IdentityResult.Failed(ErrorDescriber.ConcurrencyFailure());
            }
            return IdentityResult.Success;
        }

        /// <summary>
        /// Deletes a role from the store as an asynchronous operation.
        /// </summary>
        /// <param name="role">The role to delete from the store.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="Task{TResult}"/> that represents the <see cref="IdentityResult"/> of the asynchronous query.</returns>
        public async virtual Task<IdentityResult> DeleteAsync(TRole role, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (role == null)
            {
                throw new ArgumentNullException(nameof(role));
            }
            Context.Set<TRole>().Remove(role);
            try
            {
                await SaveChanges(cancellationToken);
            }
            catch (DbUpdateConcurrencyException)
            {
                return IdentityResult.Failed(ErrorDescriber.ConcurrencyFailure());
            }
            return IdentityResult.Success;
        }

        /// <summary>
        /// Gets the ID for a role from the store as an asynchronous operation.
        /// </summary>
        /// <param name="role">The role whose ID should be returned.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="Task{TResult}"/> that contains the ID of the role.</returns>
        public Task<string> GetRoleIdAsync(TRole role, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (role == null)
            {
                throw new ArgumentNullException(nameof(role));
            }
            return Task.FromResult(ConvertIdToString(role.Id));
        }

        /// <summary>
        /// Gets the name of a role from the store as an asynchronous operation.
        /// </summary>
        /// <param name="role">The role whose name should be returned.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="Task{TResult}"/> that contains the name of the role.</returns>
        public Task<string> GetRoleNameAsync(TRole role, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (role == null)
            {
                throw new ArgumentNullException(nameof(role));
            }
            return Task.FromResult(role.Name);
        }

        /// <summary>
        /// Sets the name of a role in the store as an asynchronous operation.
        /// </summary>
        /// <param name="role">The role whose name should be set.</param>
        /// <param name="roleName">The name of the role.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation.</returns>
        public Task SetRoleNameAsync(TRole role, string roleName, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (role == null)
            {
                throw new ArgumentNullException(nameof(role));
            }
            role.Name = roleName;
            return Task.FromResult(0);
        }

        /// <summary>
        /// Converts the provided <paramref name="id"/> to a strongly typed key object.
        /// </summary>
        /// <param name="id">The id to convert.</param>
        /// <returns>An instance of <typeparamref name="TKey"/> representing the provided <paramref name="id"/>.</returns>
        public virtual TKey ConvertIdFromString(string id)
        {
            if (id == null)
            {
                return default(TKey);
            }
            return (TKey)TypeDescriptor.GetConverter(typeof(TKey)).ConvertFromInvariantString(id);
        }

        /// <summary>
        /// Converts the provided <paramref name="id"/> to its string representation.
        /// </summary>
        /// <param name="id">The id to convert.</param>
        /// <returns>An <see cref="string"/> representation of the provided <paramref name="id"/>.</returns>
        public virtual string ConvertIdToString(TKey id)
        {
            if (id.Equals(default(TKey)))
            {
                return null;
            }
            return id.ToString();
        }

        /// <summary>
        /// Finds the role who has the specified ID as an asynchronous operation.
        /// </summary>
        /// <param name="roleId">The role ID to look for.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="Task{TResult}"/> that result of the look up.</returns>
        public virtual Task<TRole> FindByIdAsync(string id, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            var roleId = ConvertIdFromString(id);
            return Roles.FirstOrDefaultAsync(u => u.Id.Equals(roleId), cancellationToken);
        }

        /// <summary>
        /// Finds the role who has the specified normalized name as an asynchronous operation.
        /// </summary>
        /// <param name="normalizedRoleName">The normalized role name to look for.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="Task{TResult}"/> that result of the look up.</returns>
        public virtual Task<TRole> FindByNameAsync(string normalizedName, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            return Roles.FirstOrDefaultAsync(r => r.NormalizedName == normalizedName, cancellationToken);
        }

        /// <summary>
        /// Get a role's normalized name as an asynchronous operation.
        /// </summary>
        /// <param name="role">The role whose normalized name should be retrieved.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="Task{TResult}"/> that contains the name of the role.</returns>
        public virtual Task<string> GetNormalizedRoleNameAsync(TRole role, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (role == null)
            {
                throw new ArgumentNullException(nameof(role));
            }
            return Task.FromResult(role.NormalizedName);
        }

        /// <summary>
        /// Set a role's normalized name as an asynchronous operation.
        /// </summary>
        /// <param name="role">The role whose normalized name should be set.</param>
        /// <param name="normalizedName">The normalized name to set</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation.</returns>
        public virtual Task SetNormalizedRoleNameAsync(TRole role, string normalizedName, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (role == null)
            {
                throw new ArgumentNullException(nameof(role));
            }
            role.NormalizedName = normalizedName;
            return Task.FromResult(0);
        }

        private void ThrowIfDisposed()
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(GetType().Name);
            }
        }

        /// <summary>
        /// Dispose the stores
        /// </summary>
        public void Dispose()
        {
            _db.SaveChanges();
            _db.Dispose();
            _disposed = true;
        }

        /// <summary>
        /// Get the claims associated with the specified <paramref name="role"/> as an asynchronous operation.
        /// </summary>
        /// <param name="role">The role whose claims should be retrieved.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="Task{TResult}"/> that contains the claims granted to a role.</returns>
        public async Task<IList<Claim>> GetClaimsAsync(TRole role, CancellationToken cancellationToken = default(CancellationToken))
        {
            ThrowIfDisposed();
            if (role == null)
            {
                throw new ArgumentNullException(nameof(role));
            }

            return await RoleClaims.Where(rc => rc.RoleId.Equals(role.Id)).Select(c => new Claim(c.ClaimType, c.ClaimValue)).ToListAsync(cancellationToken);
        }

        /// <summary>
        /// Adds the <paramref name="claim"/> given to the specified <paramref name="role"/>.
        /// </summary>
        /// <param name="role">The role to add the claim to.</param>
        /// <param name="claim">The claim to add to the role.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation.</returns>
        public Task AddClaimAsync(TRole role, Claim claim, CancellationToken cancellationToken = default(CancellationToken))
        {
            ThrowIfDisposed();
            if (role == null)
            {
                throw new ArgumentNullException(nameof(role));
            }
            if (claim == null)
            {
                throw new ArgumentNullException(nameof(claim));
            }

            RoleClaims.Add(new IdentityRoleClaim<TKey> { RoleId = role.Id, ClaimType = claim.Type, ClaimValue = claim.Value });

            return Task.FromResult(false);
        }

        /// <summary>
        /// Removes the <paramref name="claim"/> given from the specified <paramref name="role"/>.
        /// </summary>
        /// <param name="role">The role to remove the claim from.</param>
        /// <param name="claim">The claim to remove from the role.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation.</returns>
        public async Task RemoveClaimAsync(TRole role, Claim claim, CancellationToken cancellationToken = default(CancellationToken))
        {
            ThrowIfDisposed();
            if (role == null)
            {
                throw new ArgumentNullException(nameof(role));
            }
            if (claim == null)
            {
                throw new ArgumentNullException(nameof(claim));
            }
            var claims = await RoleClaims.Where(rc => rc.RoleId.Equals(role.Id) && rc.ClaimValue == claim.Value && rc.ClaimType == claim.Type).ToListAsync(cancellationToken);
            foreach (var c in claims)
            {
                RoleClaims.Remove(c);
            }
        }

        /// <summary>
        /// A navigation property for the roles the store contains.
        /// </summary>
        public virtual IQueryable<TRole> Roles
        {
            get { return Context.Set<TRole>(); }
        }

        private DbSet<IdentityRoleClaim<TKey>> RoleClaims { get { return Context.Set<IdentityRoleClaim<TKey>>(); } }
    }
}