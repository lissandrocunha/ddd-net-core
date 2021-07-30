using Dapper;
using Infra.CrossCutting.Identity.Models;
using Microsoft.AspNetCore.Identity;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace Infra.CrossCutting.Identity.Repositories
{
    public class ApplicationUserStore : IUserStore<ApplicationUser>,
                                        IUserRoleStore<ApplicationUser>,
                                        IUserClaimStore<ApplicationUser>,
                                        IUserLoginStore<ApplicationUser>,
                                        IUserPasswordStore<ApplicationUser>,
                                        IUserSecurityStampStore<ApplicationUser>
    {

        #region Variables

        private readonly string _connectionString;

        #endregion

        #region Constructors

        public ApplicationUserStore(string connectionString)
        {
            _connectionString = connectionString;
        }

        #endregion

        #region Methods

        #region IUserStore

        public Task<ApplicationUser> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            using (OracleConnection connection = new OracleConnection(_connectionString))
            {
                var sql = @"SELECT
                                    u.nom_usuario_banco                                   
                                   ,u.nom_usuario 
                                   ,u.sbn_usuario
                                   ,u.num_documento                                   
                                   ,u.dsc_chave_acesso
                                   ,u.dsc_senha_pda
                                   ,u.dta_cadastro
                                   ,u.dta_expiracao
                                   ,u.dta_ultima_conexao
                                   ,u.idf_nivel_contingencia
                                   ,u.idf_tipo_doc_apre
                                   ,u.idf_tipo_documento
                                   ,u.idf_tipo_usuario
                                   ,u.num_cracha
                                   ,u.num_user_banco
                                   ,u.num_user_principal
                                   ,c.dsc_email
                              FROM
                                    acesso.usuario u
                                   ,estoque.complemento_usuario c
                             WHERE
                                   1                  = 1
                               AND c.num_user_banco   = u.num_user_banco
                               AND u.num_user_banco   = :num_user_banco";

                connection.OpenAsync();

                var result = connection.QueryFirstOrDefaultAsync(
                    sql,
                    new
                    {
                        num_user_banco = userId
                    }).Result;

                return result == null ? result : Task.FromResult(ApplicationUser.Factory.Novo(
                    ((decimal)result.NUM_USER_BANCO).ToString(),
                    (string)result.NOM_USUARIO + (string.IsNullOrWhiteSpace(result.SBN_USUARIO) ? "" : " " + (string)result.SBN_USUARIO),
                    (string)result.NOM_USUARIO_BANCO,
                    ((string)result.DSC_EMAIL)?.ToLower(),
                    (int)result.IDF_TIPO_USUARIO,
                    ((DateTime?)result.DTA_EXPIRACAO)?.ToString()
                    ));
            }
        }

        public Task<ApplicationUser> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            using (OracleConnection connection = new OracleConnection(_connectionString))
            {
                var sql = @"SELECT
                                    u.nom_usuario_banco                                   
                                   ,u.nom_usuario 
                                   ,u.sbn_usuario
                                   ,u.num_documento                                   
                                   ,u.dsc_chave_acesso
                                   ,u.dsc_senha_pda
                                   ,u.dta_cadastro
                                   ,u.dta_expiracao
                                   ,u.dta_ultima_conexao
                                   ,u.idf_nivel_contingencia
                                   ,u.idf_tipo_doc_apre
                                   ,u.idf_tipo_documento
                                   ,u.idf_tipo_usuario
                                   ,u.num_cracha
                                   ,u.num_user_banco
                                   ,u.num_user_principal
                                   ,c.dsc_email
                              FROM
                                    acesso.usuario u
                                   ,estoque.complemento_usuario c
                             WHERE
                                   1                  = 1
                               AND c.num_user_banco      = u.num_user_banco
                               AND UPPER(u.nom_usuario_banco)   = UPPER(:nom_usuario_banco)";

                connection.OpenAsync();

                var result = connection.QueryFirstOrDefaultAsync(
                    sql,
                new
                {
                    nom_usuario_banco = normalizedUserName
                }).Result;

                return result == null ? result : Task.FromResult(ApplicationUser.Factory.Novo(
                    ((decimal)result.NUM_USER_BANCO).ToString(),
                    ((string.IsNullOrWhiteSpace(result.NOM_USUARIO) ? "" : ((string)result.NOM_USUARIO)).Trim()) + (string.IsNullOrWhiteSpace(result.SBN_USUARIO) ? "" : " " + (string)result.SBN_USUARIO),
                    (string)result.NOM_USUARIO_BANCO,
                    ((string)result.DSC_EMAIL)?.ToLower(),
                    (int)result.IDF_TIPO_USUARIO,
                    ((DateTime?)result.DTA_EXPIRACAO)?.ToString(),
                    document: (long.TryParse((string)result.NUM_DOCUMENTO, out long cpf) ? (long?)cpf : null)
                    ));
            }
        }

        public Task<string> GetNormalizedUserNameAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.NormalizedUserName);
        }

        public Task<string> GetUserIdAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            //ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            return Task.FromResult(user.Id.ToString());
        }

        public Task<byte[]> GetUserPhotoAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            using (OracleConnection connection = new OracleConnection(_connectionString))
            {
                var sql = @"SELECT
                                    v.fotemp
                              FROM
                                    v_busca_foto_cracha v
                             WHERE
                                    1 = 1
                                AND v.num_user_banco = :num_user_banco
                                AND ROWNUM = 1";

                // necessário para erro TTC nos campos BLOBs
                //var sql = @"WITH INFO AS (
                //                                 SELECT
                //                                         V.FOTEMP
                //                                         ,DBMS_LOB.GETLENGTH(
                //                                                 V.FOTEMP
                //                                         ) AS FILE_CONTENT_LENGTH
                //                                         ,MOD(
                //                                                 DBMS_LOB.GETLENGTH(
                //                                                         V.FOTEMP
                //                                                 )
                //                                                 ,2000
                //                                         ) AS MOD
                //                                         ,CASE
                //                                                 WHEN MOD(
                //                                                         DBMS_LOB.GETLENGTH(
                //                                                                 V.FOTEMP
                //                                                         )
                //                                                         ,2000
                //                                                 ) > 0
                //                                                 THEN TRUNC((DBMS_LOB.GETLENGTH(
                //                                                         V.FOTEMP
                //                                                 ) / 2000) + 1)
                //                                                 ELSE TRUNC(DBMS_LOB.GETLENGTH(
                //                                                         V.FOTEMP
                //                                                 ) / 2000)
                //                                         END INTERATION_COUNT
                //                                   FROM
                //                                         V_BUSCA_FOTO_CRACHA V
                //                                  WHERE
                //                                         V.NUMEMP = 1
                //                                            AND V.NUM_USER_BANCO = :num_user_banco
                //                                            AND ROWNUM           = 1
                //                         ),OFFSETS AS (
                //                                 SELECT
                //                                         ( 2000 * ( ROWNUM - 1 ) ) + 1 AS OFFSET
                //                                         ,I.MOD
                //                                         ,I.FILE_CONTENT_LENGTH
                //                                         ,I.FOTEMP
                //                                         ,I.INTERATION_COUNT
                //                                   FROM
                //                                         INFO I
                //                                 CONNECT BY
                //                                         LEVEL <= I.INTERATION_COUNT
                //                         ),RESULT AS (
                //                                 SELECT
                //                                         DBMS_LOB.SUBSTR(
                //                                                 O.FOTEMP
                //                                                 ,2000
                //                                                 ,O.OFFSET
                //                                         ) AS CONTENT
                //                                         ,O.OFFSET
                //                                         ,O.MOD
                //                                         ,O.FILE_CONTENT_LENGTH
                //                                         ,O.INTERATION_COUNT
                //                                   FROM
                //                                         OFFSETS O
                //                         )
                //                         SELECT
                //                                 R.OFFSET
                //                                 ,R.CONTENT
                //                           FROM
                //                                 RESULT R
                //                          ORDER BY
                //                                 R.OFFSET ASC ";

                connection.OpenAsync();

                byte[] result = null;

                var bytesFoto = connection.QueryAsync<dynamic>(
                    sql,
                    new
                    {
                        num_user_banco = user.Id
                    }).Result;

                // montar o byte array
                if (bytesFoto != null
                 && bytesFoto.FirstOrDefault() != null
                 && bytesFoto.FirstOrDefault().FOTEMP != null
                 && ((byte[])bytesFoto.FirstOrDefault().FOTEMP).Length > 0)
                {
                    result = bytesFoto.FirstOrDefault().FOTEMP;
                    //result = bytesFoto.Select(x => x.CONTENT).Cast<byte[]>().SelectMany(a => a).ToArray();
                }

                return Task.FromResult(result);
            }
        }

        public Task<string> GetUserNameAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.UserName);
        }

        public Task<IdentityResult> CreateAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IdentityResult> DeleteAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task SetNormalizedUserNameAsync(ApplicationUser user, string normalizedName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task SetUserNameAsync(ApplicationUser user, string userName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IdentityResult> UpdateAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IUserRoleStore

        public Task AddToRoleAsync(ApplicationUser user, string roleName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task RemoveFromRoleAsync(ApplicationUser user, string roleName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IList<string>> GetRolesAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            using (OracleConnection connection = new OracleConnection(_connectionString))
            {
                var sql = @"SELECT DISTINCT
                                    R.NOM_ROLE Name
                              FROM
                                     acesso.usuario_role ur
                                    ,acesso.role r
                             WHERE
                                   1                  = 1
                               AND r.cod_role          = ur.cod_role
                               AND ur.num_user_banco   = :num_user_banco ";

                if (!string.IsNullOrWhiteSpace(user.SystemCod))
                    sql += "AND r.cod_sistema       = :cod_sistema";

                var dynamicParameters = new DynamicParameters();
                dynamicParameters.Add("num_user_banco", user.Id);
                if (!string.IsNullOrWhiteSpace(user.SystemCod))
                    dynamicParameters.Add("cod_sistema", user.SystemCod);

                connection.OpenAsync();

                var result = connection.QueryAsync<string>(
                    sql,
                    dynamicParameters
                ).Result.ToList();

                return Task.FromResult((IList<string>)result);
            }
        }

        public Task<bool> IsInRoleAsync(ApplicationUser user, string roleName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IList<ApplicationUser>> GetUsersInRoleAsync(string roleName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IUserClaimStore

        public Task<IList<Claim>> GetClaimsAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            using (OracleConnection connection = new OracleConnection(_connectionString))
            {
                var sql = @"SELECT DISTINCT
                                    TO_CHAR(ur.cod_role)          Type
                                   ,r.nom_role                    Value
                                   ,TO_CHAR(r.cod_sistema)        Issuer
                                   ,TO_CHAR(r.cod_role_pai)       ValueType
                                   ,TO_CHAR(ur.cod_inst_sistema)  OriginalIssuer
                              FROM
                                    acesso.usuario_role ur
                                   ,acesso.role r
                             WHERE
                                   1                  = 1
                               AND r.cod_role          = ur.cod_role
                               AND ur.num_user_banco   = :num_user_banco ";

                if (!string.IsNullOrWhiteSpace(user.SystemCod))
                    sql += "AND r.cod_sistema       = :cod_sistema";

                var dynamicParameters = new DynamicParameters();
                dynamicParameters.Add("num_user_banco", user.Id);
                if (!string.IsNullOrWhiteSpace(user.SystemCod))
                    dynamicParameters.Add("cod_sistema", user.SystemCod);

                connection.OpenAsync();

                var result = connection.QueryAsync(
                    sql,
                    dynamicParameters)
                    .Result
                    .Select(row =>
                    new Claim(
                        (string)row.TYPE,
                        (string)row.VALUE,
                        (string)row.VALUETYPE,
                        (string)row.ISSUER,
                        (string)row.ORIGINALISSUER))
                    .ToList();

                return Task.FromResult((IList<Claim>)result);
            }
        }

        public Task AddClaimsAsync(ApplicationUser user, IEnumerable<Claim> claims, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task ReplaceClaimAsync(ApplicationUser user, Claim claim, Claim newClaim, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task RemoveClaimsAsync(ApplicationUser user, IEnumerable<Claim> claims, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IList<ApplicationUser>> GetUsersForClaimAsync(Claim claim, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IUserLoginStore

        public Task AddLoginAsync(ApplicationUser user, UserLoginInfo login, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task RemoveLoginAsync(ApplicationUser user, string loginProvider, string providerKey, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IList<UserLoginInfo>> GetLoginsAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<ApplicationUser> FindByLoginAsync(string loginProvider, string providerKey, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IUserPasswordStore

        public Task SetPasswordHashAsync(ApplicationUser user, string passwordHash, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetPasswordHashAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.PasswordHash);
        }

        public Task<bool> HasPasswordAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IUserSecurityStampStore

        public Task SetSecurityStampAsync(ApplicationUser user, string stamp, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetSecurityStampAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        #endregion

        internal async Task<SignInResult> CheckOracleConnectionAsync(string userName, string password, CancellationToken cancellationToken)
        {

            string newConnectionString = string.Format("User Id = {0}; Password = {1}; {2}", userName, password, string.Join(';', _connectionString.Split(";").Where(s => !s.Contains("User") && !s.Contains("Password")).ToArray()));

            using (OracleConnection conn = new OracleConnection(newConnectionString))
            {
                try
                {
                    await conn.OpenAsync();
                }
                catch (Exception)
                {
                    return await Task.FromResult(new SignInResult());
                }
            }

            return await Task.FromResult(new SuccessSignResult() as SignInResult);
        }

        internal async Task<string> CryptoOraclePasswordAsync(string password, CancellationToken cancellationToken)
        {

            using (OracleConnection connection = new OracleConnection(_connectionString))
            {
                try
                {
                    await connection.OpenAsync();

                    return await connection.QueryFirstOrDefaultAsync<string>(
                        "SELECT fcn_crypthc(:senha,'E',0) x FROM dual",
                    new
                    {
                        senha = password
                    });

                }
                catch (Exception)
                {
                    return null;
                }
            }

        }


        public void Dispose()
        {

        }

        #endregion

    }

    public class SuccessSignResult : SignInResult
    {

        #region Constructors

        public SuccessSignResult(bool isLockedOut = false,
                                 bool isNotAllowed = false,
                                 bool requiresTwoFactor = false)
        {
            Succeeded = true;
            IsLockedOut = isLockedOut;
            IsNotAllowed = isNotAllowed;
            RequiresTwoFactor = requiresTwoFactor;
        }

        #endregion

    }
}
