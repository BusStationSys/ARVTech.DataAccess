﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARVTech.DataAccess.CQRS.Queries
{
    public class PessoaQuery : BaseQuery
    {
        // To detect redundant calls.
        private bool _disposedValue = false;

        private readonly string _columnsPessoas;

        public override string CommandTextCreate()
        {
            throw new NotImplementedException();
        }

        public override string CommandTextDelete()
        {
            throw new NotImplementedException();
        }

        public override string CommandTextGetAll()
        {
            throw new NotImplementedException();
        }

        public override string CommandTextGetById()
        {
            throw new NotImplementedException();
        }

        public override string CommandTextUpdate()
        {
            return $@"     UPDATE [dbo].[{base.TableNamePessoas}]
                              SET [BAIRRO] = @Bairro,
                                  [CEP] = @Cep,
                                  [CIDADE] = @Cidade,
                                  [COMPLEMENTO] = @Complemento,
                                  [ENDERECO] = @Endereco,
                                  [NUMERO] = @Numero,
                                  [UF] = @Uf
                            WHERE [GUID] = @GuidPessoa ";
        }

        public PessoaQuery(SqlConnection connection, SqlTransaction? transaction = null) :
            base(connection, transaction)
        {
            this._columnsPessoas = base.GetAllColumnsFromTable(
                base.TableNamePessoas,
                base.TableAliasPessoas);
        }

        // Protected implementation of Dispose pattern. https://learn.microsoft.com/en-us/dotnet/standard/garbage-collection/implementing-dispose
        protected override void Dispose(bool disposing)
        {
            if (!this._disposedValue)
            {
                if (disposing)
                {
                    //  TODO: dispose managed state (managed objects).
                }

                this._disposedValue = true;
            }

            // Call base class implementation.
            base.Dispose(disposing);
        }
    }
}