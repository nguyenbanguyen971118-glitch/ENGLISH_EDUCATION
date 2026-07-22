using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class CheckYeucaulichdaySync : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // No-op: GhiChuAdmin/LoaiYeuCau/MaPhongHocDeXuat + FK already exist physically
            // (added by the AddScheduleChangeRequestColumns migration). They were previously
            // hidden from EF via .Ignore() in AppDbContext, which regenerated the model
            // snapshot without them. This migration only re-syncs EF's bookkeeping with the
            // real schema now that the .Ignore() calls have been removed — it must not repeat
            // the DDL, since the columns/FK already exist.
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
        }
    }
}
