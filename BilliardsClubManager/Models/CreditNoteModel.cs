using BilliardsClubManager.Base;
using Dapper;
using Dapper.Contrib.Extensions;
using NullVoidCreations.WpfHelpers;
using NullVoidCreations.WpfHelpers.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace BilliardsClubManager.Models
{
    [Table("CreditNotes")]
    class CreditNoteModel: NotificationBase, IRecord
    {
        long _id;
        PlayerModel _player;
        DateTime? _date;
        decimal _amount;
        string _notes;

        public CreditNoteModel()
        {
            Id = -1;
            Date = DateTime.Now;
        }

        #region properties

        [Key]
        public long Id
        {
            get => _id;
            set => Set(nameof(Id), ref _id, value);
        }

        [DisplayName("Player")]
        public PlayerModel Player
        {
            get => _player;
            set => Set(nameof(Player), ref _player, value);
        }

        [DisplayName("Date & Time")]
        public DateTime? Date
        {
            get => _date;
            set => Set(nameof(Date), ref _date, value);
        }

        [DisplayName("Amount (in ₹)")]
        public decimal Amount
        {
            get => _amount;
            set => Set(nameof(Amount), ref _amount, value);
        }

        [DisplayName("Notes")]
        public string Notes
        {
            get => _notes;
            set => Set(nameof(Notes), ref _notes, value);
        }

        #endregion

        CreditNoteModel MapRecord(CreditNoteModel creditNote, PlayerModel player)
        {
            creditNote.Player = player;
            return creditNote;
        }

        public string Delete()
        {
            using (var connection = Shared.Instance.GetConnection())
            {
                return connection.Delete(this) ? null : "Failed to delete record.";
            }
        }

        public IRecord Get(long id)
        {
            using (var connection = Shared.Instance.GetConnection())
            {
                return connection.Get<CreditNoteModel>(id);
            }
        }

        public IEnumerable<IRecord> Get(string searchKeywoard)
        {
            var sqlbuilder = new StringBuilder();
            sqlbuilder.AppendLineFormatted("SELECT");
            sqlbuilder.AppendLineFormatted("  *");
            sqlbuilder.AppendLineFormatted("FROM [CreditNotes] AS C");
            sqlbuilder.AppendLineFormatted("INNER JOIN [Players] AS P ON C.PlayerId = P.Id");
            sqlbuilder.AppendLineFormatted("WHERE");
            sqlbuilder.AppendLineFormatted("  P.Name LIKE '%{0}%' OR", searchKeywoard);
            sqlbuilder.AppendLineFormatted("  C.Notes LIKE '%{0}%'", searchKeywoard);

            using (var connection = Shared.Instance.GetConnection())
            {
                return connection.Query<CreditNoteModel, PlayerModel, CreditNoteModel>(sqlbuilder.ToString(), MapRecord);
            }
        }

        public IRecord New()
        {
            return new CreditNoteModel();
        }

        public string Save()
        {
            if (Player == null)
                return "Player not specified.";
            if (Date == null)
                return "Date & time not specified.";
            if (Amount <= 0)
                return "Amount not specified.";
            
            using (var connection = Shared.Instance.GetConnection())
            {
                bool isSaved;
                var sqlBuilder = new StringBuilder();

                if (Id < 0)
                {
                    sqlBuilder.AppendLineFormatted("INSERT INTO [CreditNotes] (PlayerId, Date, Amount, Notes)");
                    sqlBuilder.AppendLineFormatted("VALUES ('{0}', '{1}', {2}, '{3}');", Player.Id, Date.Value, Amount, Notes);
                    sqlBuilder.AppendLineFormatted("SELECT last_insert_rowid();");

                    Id = connection.ExecuteScalar<long>(sqlBuilder.ToString());
                    isSaved = Id > -1;
                }
                else
                {
                    sqlBuilder.AppendLineFormatted("UPDATE [CreditNotes]");
                    sqlBuilder.AppendLineFormatted("SET");
                    sqlBuilder.AppendLineFormatted("  [PlayerId] = {0},", Player.Id);
                    sqlBuilder.AppendLineFormatted("  [Date] = '{0}',", Date.Value);
                    sqlBuilder.AppendLineFormatted("  [Amount] = {0},", Amount);
                    sqlBuilder.AppendLineFormatted("  [Notes] = '{0}'", Notes);
                    sqlBuilder.AppendLineFormatted("WHERE [Id] = {0}", Id);

                    isSaved = connection.Execute(sqlBuilder.ToString()) > 0;
                }

                return isSaved ? null : "Failed to save record.";
            }
        }
    }
}
