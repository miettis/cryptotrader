using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace CryptoTrader.Data.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "config",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    type = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    value = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_config", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "crypto",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    symbol = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    rank = table.Column<int>(type: "integer", nullable: false),
                    trade = table.Column<bool>(type: "boolean", nullable: false),
                    max_purchase = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    max_share = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    followed = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    times_start = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    times_start_data = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    times_end_data = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    times_end_ma = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    times_end_slope = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    times_end_return = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_crypto", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "order",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    binance_id = table.Column<long>(type: "bigint", nullable: false),
                    created = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    updated = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    status = table.Column<int>(type: "integer", nullable: false),
                    side = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    symbol = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    price = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: false),
                    quantity = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: false),
                    executed_quantity = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    quote_quantity = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    average_fill_price = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    commission = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    quantity_remaining = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    create_response = table.Column<string>(type: "text", nullable: true),
                    cancel_response = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_order", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "crypto_statistics",
                columns: table => new
                {
                    crypto_id = table.Column<int>(type: "integer", nullable: false),
                    start_time = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    end_time = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    candle_length_mean = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: false),
                    candle_length_std = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: false),
                    candle_length_lim5 = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: false),
                    candle_length_lim10 = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: false),
                    candle_length_lim25 = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: false),
                    candle_length_lim50 = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: false),
                    candle_length_lim75 = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: false),
                    candle_length_lim90 = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: false),
                    candle_length_lim95 = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: false),
                    candle_proportion_mean = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: false),
                    candle_proportion_std = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: false),
                    candle_proportion_lim5 = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: false),
                    candle_proportion_lim10 = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: false),
                    candle_proportion_lim25 = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: false),
                    candle_proportion_lim50 = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: false),
                    candle_proportion_lim75 = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: false),
                    candle_proportion_lim90 = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: false),
                    candle_proportion_lim95 = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: false),
                    upper_length_mean = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: false),
                    upper_length_std = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: false),
                    upper_length_lim5 = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: false),
                    upper_length_lim10 = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: false),
                    upper_length_lim25 = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: false),
                    upper_length_lim50 = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: false),
                    upper_length_lim75 = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: false),
                    upper_length_lim90 = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: false),
                    upper_length_lim95 = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: false),
                    upper_proportion_mean = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: false),
                    upper_proportion_std = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: false),
                    upper_proportion_lim5 = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: false),
                    upper_proportion_lim10 = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: false),
                    upper_proportion_lim25 = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: false),
                    upper_proportion_lim50 = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: false),
                    upper_proportion_lim75 = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: false),
                    upper_proportion_lim90 = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: false),
                    upper_proportion_lim95 = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: false),
                    body_length_mean = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: false),
                    body_length_std = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: false),
                    body_length_lim5 = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: false),
                    body_length_lim10 = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: false),
                    body_length_lim25 = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: false),
                    body_length_lim50 = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: false),
                    body_length_lim75 = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: false),
                    body_length_lim90 = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: false),
                    body_length_lim95 = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: false),
                    body_proportion_mean = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: false),
                    body_proportion_std = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: false),
                    body_proportion_lim5 = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: false),
                    body_proportion_lim10 = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: false),
                    body_proportion_lim25 = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: false),
                    body_proportion_lim50 = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: false),
                    body_proportion_lim75 = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: false),
                    body_proportion_lim90 = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: false),
                    body_proportion_lim95 = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: false),
                    lower_length_mean = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: false),
                    lower_length_std = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: false),
                    lower_length_lim5 = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: false),
                    lower_length_lim10 = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: false),
                    lower_length_lim25 = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: false),
                    lower_length_lim50 = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: false),
                    lower_length_lim75 = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: false),
                    lower_length_lim90 = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: false),
                    lower_length_lim95 = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: false),
                    lower_proportion_mean = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: false),
                    lower_proportion_std = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: false),
                    lower_proportion_lim5 = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: false),
                    lower_proportion_lim10 = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: false),
                    lower_proportion_lim25 = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: false),
                    lower_proportion_lim50 = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: false),
                    lower_proportion_lim75 = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: false),
                    lower_proportion_lim90 = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: false),
                    lower_proportion_lim95 = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_crypto_statistics", x => new { x.crypto_id, x.start_time, x.end_time });
                    table.ForeignKey(
                        name: "fk_crypto_statistics_cryptos_crypto_id",
                        column: x => x.crypto_id,
                        principalTable: "crypto",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "price",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    crypto_id = table.Column<int>(type: "integer", nullable: false),
                    timestamp = table.Column<long>(type: "bigint", nullable: false),
                    time_open = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    open = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: false),
                    high = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: false),
                    low = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: false),
                    close = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: false),
                    volume = table.Column<decimal>(type: "numeric(38,18)", precision: 38, scale: 18, nullable: false),
                    quote_volume = table.Column<decimal>(type: "numeric(38,18)", precision: 38, scale: 18, nullable: false),
                    trades = table.Column<long>(type: "bigint", nullable: true),
                    buy_volume = table.Column<decimal>(type: "numeric(38,18)", precision: 38, scale: 18, nullable: false),
                    buy_quote_volume = table.Column<decimal>(type: "numeric(38,18)", precision: 38, scale: 18, nullable: false),
                    market_cap = table.Column<decimal>(type: "numeric(38,18)", precision: 38, scale: 18, nullable: false),
                    avg_ohlc = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: false),
                    avg_hlc = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: false),
                    avg_hl = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: false),
                    avg_oc = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: false),
                    proportion_upper = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: false),
                    proportion_body = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: false),
                    proportion_lower = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: false),
                    length_candle = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: false),
                    length_upper = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: false),
                    length_body = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: false),
                    length_lower = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_price", x => x.id);
                    table.ForeignKey(
                        name: "fk_price_crypto_crypto_id",
                        column: x => x.crypto_id,
                        principalTable: "crypto",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "price_prediction",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    crypto_id = table.Column<int>(type: "integer", nullable: false),
                    created = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    time_open = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    open = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: false),
                    high = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: false),
                    low = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: false),
                    close = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_price_prediction", x => x.id);
                    table.ForeignKey(
                        name: "fk_price_prediction_crypto_crypto_id",
                        column: x => x.crypto_id,
                        principalTable: "crypto",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "price_candle_sticks",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false),
                    ab_bear = table.Column<bool>(type: "boolean", nullable: false),
                    ab_bull = table.Column<bool>(type: "boolean", nullable: false),
                    ats = table.Column<bool>(type: "boolean", nullable: false),
                    adv = table.Column<bool>(type: "boolean", nullable: false),
                    bts = table.Column<bool>(type: "boolean", nullable: false),
                    bh_bear = table.Column<bool>(type: "boolean", nullable: false),
                    bh_bull = table.Column<bool>(type: "boolean", nullable: false),
                    ba_bear = table.Column<bool>(type: "boolean", nullable: false),
                    ba_bull = table.Column<bool>(type: "boolean", nullable: false),
                    cb = table.Column<bool>(type: "boolean", nullable: false),
                    csb = table.Column<bool>(type: "boolean", nullable: false),
                    csw = table.Column<bool>(type: "boolean", nullable: false),
                    cw = table.Column<bool>(type: "boolean", nullable: false),
                    cbs = table.Column<bool>(type: "boolean", nullable: false),
                    ca_bear = table.Column<bool>(type: "boolean", nullable: false),
                    ca_bull = table.Column<bool>(type: "boolean", nullable: false),
                    dcc = table.Column<bool>(type: "boolean", nullable: false),
                    del = table.Column<bool>(type: "boolean", nullable: false),
                    doji = table.Column<bool>(type: "boolean", nullable: false),
                    dd = table.Column<bool>(type: "boolean", nullable: false),
                    dgd = table.Column<bool>(type: "boolean", nullable: false),
                    dgu = table.Column<bool>(type: "boolean", nullable: false),
                    dgs = table.Column<bool>(type: "boolean", nullable: false),
                    dll = table.Column<bool>(type: "boolean", nullable: false),
                    dn = table.Column<bool>(type: "boolean", nullable: false),
                    ds = table.Column<bool>(type: "boolean", nullable: false),
                    ds_bear = table.Column<bool>(type: "boolean", nullable: false),
                    ds_bull = table.Column<bool>(type: "boolean", nullable: false),
                    dsc = table.Column<bool>(type: "boolean", nullable: false),
                    dgtm = table.Column<bool>(type: "boolean", nullable: false),
                    dtg = table.Column<bool>(type: "boolean", nullable: false),
                    eng_bear = table.Column<bool>(type: "boolean", nullable: false),
                    eng_bull = table.Column<bool>(type: "boolean", nullable: false),
                    eds = table.Column<bool>(type: "boolean", nullable: false),
                    es = table.Column<bool>(type: "boolean", nullable: false),
                    ftm = table.Column<bool>(type: "boolean", nullable: false),
                    hammer = table.Column<bool>(type: "boolean", nullable: false),
                    inv = table.Column<bool>(type: "boolean", nullable: false),
                    hm = table.Column<bool>(type: "boolean", nullable: false),
                    ha_bear = table.Column<bool>(type: "boolean", nullable: false),
                    ha_bull = table.Column<bool>(type: "boolean", nullable: false),
                    hc_bear = table.Column<bool>(type: "boolean", nullable: false),
                    hc_bull = table.Column<bool>(type: "boolean", nullable: false),
                    hw_bear = table.Column<bool>(type: "boolean", nullable: false),
                    hw_bull = table.Column<bool>(type: "boolean", nullable: false),
                    hik_bear = table.Column<bool>(type: "boolean", nullable: false),
                    hik_bull = table.Column<bool>(type: "boolean", nullable: false),
                    hmod_bear = table.Column<bool>(type: "boolean", nullable: false),
                    hmod_bull = table.Column<bool>(type: "boolean", nullable: false),
                    hp = table.Column<bool>(type: "boolean", nullable: false),
                    itc = table.Column<bool>(type: "boolean", nullable: false),
                    @in = table.Column<bool>(name: "in", type: "boolean", nullable: false),
                    ki_bear = table.Column<bool>(type: "boolean", nullable: false),
                    ki_bull = table.Column<bool>(type: "boolean", nullable: false),
                    kl_bear = table.Column<bool>(type: "boolean", nullable: false),
                    kl_bull = table.Column<bool>(type: "boolean", nullable: false),
                    lab = table.Column<bool>(type: "boolean", nullable: false),
                    leb = table.Column<bool>(type: "boolean", nullable: false),
                    let = table.Column<bool>(type: "boolean", nullable: false),
                    lbd = table.Column<bool>(type: "boolean", nullable: false),
                    lwd = table.Column<bool>(type: "boolean", nullable: false),
                    mb = table.Column<bool>(type: "boolean", nullable: false),
                    mcb = table.Column<bool>(type: "boolean", nullable: false),
                    mcw = table.Column<bool>(type: "boolean", nullable: false),
                    mob = table.Column<bool>(type: "boolean", nullable: false),
                    mow = table.Column<bool>(type: "boolean", nullable: false),
                    mw = table.Column<bool>(type: "boolean", nullable: false),
                    ml = table.Column<bool>(type: "boolean", nullable: false),
                    math = table.Column<bool>(type: "boolean", nullable: false),
                    ml_bear = table.Column<bool>(type: "boolean", nullable: false),
                    ml_bull = table.Column<bool>(type: "boolean", nullable: false),
                    mds = table.Column<bool>(type: "boolean", nullable: false),
                    ms = table.Column<bool>(type: "boolean", nullable: false),
                    on = table.Column<bool>(type: "boolean", nullable: false),
                    pp = table.Column<bool>(type: "boolean", nullable: false),
                    rm = table.Column<bool>(type: "boolean", nullable: false),
                    rtm = table.Column<bool>(type: "boolean", nullable: false),
                    sel_bear = table.Column<bool>(type: "boolean", nullable: false),
                    sel_bull = table.Column<bool>(type: "boolean", nullable: false),
                    ssoc = table.Column<bool>(type: "boolean", nullable: false),
                    sstc = table.Column<bool>(type: "boolean", nullable: false),
                    shl_bear = table.Column<bool>(type: "boolean", nullable: false),
                    shl_bull = table.Column<bool>(type: "boolean", nullable: false),
                    sbwl_bear = table.Column<bool>(type: "boolean", nullable: false),
                    sbwl_bull = table.Column<bool>(type: "boolean", nullable: false),
                    stb = table.Column<bool>(type: "boolean", nullable: false),
                    stw = table.Column<bool>(type: "boolean", nullable: false),
                    sp = table.Column<bool>(type: "boolean", nullable: false),
                    ss = table.Column<bool>(type: "boolean", nullable: false),
                    tl = table.Column<bool>(type: "boolean", nullable: false),
                    tbc = table.Column<bool>(type: "boolean", nullable: false),
                    tid = table.Column<bool>(type: "boolean", nullable: false),
                    tiu = table.Column<bool>(type: "boolean", nullable: false),
                    tls_bear = table.Column<bool>(type: "boolean", nullable: false),
                    tls_bull = table.Column<bool>(type: "boolean", nullable: false),
                    tod = table.Column<bool>(type: "boolean", nullable: false),
                    tou = table.Column<bool>(type: "boolean", nullable: false),
                    tsits = table.Column<bool>(type: "boolean", nullable: false),
                    tws = table.Column<bool>(type: "boolean", nullable: false),
                    thru = table.Column<bool>(type: "boolean", nullable: false),
                    ts_bear = table.Column<bool>(type: "boolean", nullable: false),
                    ts_bull = table.Column<bool>(type: "boolean", nullable: false),
                    twb = table.Column<bool>(type: "boolean", nullable: false),
                    twt = table.Column<bool>(type: "boolean", nullable: false),
                    tbgc = table.Column<bool>(type: "boolean", nullable: false),
                    tc = table.Column<bool>(type: "boolean", nullable: false),
                    utrb = table.Column<bool>(type: "boolean", nullable: false),
                    ugswl_bear = table.Column<bool>(type: "boolean", nullable: false),
                    ugswl_bull = table.Column<bool>(type: "boolean", nullable: false),
                    ugtm = table.Column<bool>(type: "boolean", nullable: false),
                    ugc = table.Column<bool>(type: "boolean", nullable: false),
                    utg = table.Column<bool>(type: "boolean", nullable: false),
                    wif = table.Column<bool>(type: "boolean", nullable: false),
                    wir = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_price_candle_sticks", x => x.id);
                    table.ForeignKey(
                        name: "fk_price_candle_sticks_price_id",
                        column: x => x.id,
                        principalTable: "price",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "price_cycles",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false),
                    eacc_cycle = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    eacc_period = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    eacp = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    ecfse = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    ecc = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    eca = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    ecbpf = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    edftse = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    edddc = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    ebsi = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    efsa_wave = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    efsa_roc = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    ehdc = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    eipi = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    epadc = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    esci = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    eswi1_sine = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    eswi1_lead = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    eswi2_sine = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    eswi2_lead = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    esdfb = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    esi = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    escc_cycle = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    escc_signal = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    ezcdc = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    glco = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: false),
                    hcc_fast_upper = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    hcc_slow_upper = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    hcc_fast_middle = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    hcc_slow_middle = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    hcc_fast_lower = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    hcc_slow_lower = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    hcc_omed = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    hcc_oshort = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    sc = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_price_cycles", x => x.id);
                    table.ForeignKey(
                        name: "fk_price_cycles_price_id",
                        column: x => x.id,
                        principalTable: "price",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "price_ma",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false),
                    alma6 = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    alma12 = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    alma24 = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    alma168 = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    dema6 = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    dema12 = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    dema24 = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    dema168 = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    ema6 = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    ema12 = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    ema24 = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    ema168 = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    epma6 = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    epma12 = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    epma24 = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    epma168 = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    hma6 = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    hma12 = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    hma24 = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    hma168 = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    kama_er = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    kama_kama = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    mama_mama = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    mama_fama = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    sma12_sma = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    sma12_mad = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    sma12_mse = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    sma12_mape = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    sma24_sma = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    sma24_mad = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    sma24_mse = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    sma24_mape = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    sma168_sma = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    sma168_mad = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    sma168_mse = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    sma168_mape = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    smma6 = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    smma12 = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    smma24 = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    smma168 = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    t3_6 = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    t3_12 = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    t3_246 = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    t3_168 = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    tema6 = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    tema12 = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    tema24 = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    tema168 = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    wma6 = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    wma12 = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    wma24 = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    wma168 = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    vwma6 = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    vwma12 = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    vwma24 = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    vwma168 = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_price_ma", x => x.id);
                    table.ForeignKey(
                        name: "fk_price_ma_price_id",
                        column: x => x.id,
                        principalTable: "price",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "price_momentums",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false),
                    apo = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    ao_osc = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    ao_norm = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    bop = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    cmo = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    cci = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    csi_csi = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    csi_signal = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    crsi_rsi = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    crsi_streak = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    crsi_rank = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    crsi_crsi = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    dpo_sma = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    dpo_dpo = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    eri_bull = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    eri_bear = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    go_upper = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    go_lower = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    go_upper_exp = table.Column<bool>(type: "boolean", nullable: false),
                    go_lower_exp = table.Column<bool>(type: "boolean", nullable: false),
                    macd_macd = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    macd_signal = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    macd_hist = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    obv_obv = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    obv_sma = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    ppo_ppo = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    ppo_signal = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    ppo_hist = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    pmo_pmo = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    pmo_signal = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    roc_momentum = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    roc_roc = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    roc_sma = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    rsi = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    rocwb_roc = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    rocwb_ema = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    rocwb_upper = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    rocwb_lower = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    stc = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    sfo_ppo = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    sfo_signal = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    smi_smi = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    smi_signal = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    stoch_k = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    stoch_d = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    stoch_j = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    stochrsi_rsi = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    stochrsi_signal = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    trix_ema3 = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    trix_trix = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    trix_signal = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    tsi_tsi = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    tsi_signal = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    ultimate = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    williamsr = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_price_momentums", x => x.id);
                    table.ForeignKey(
                        name: "fk_price_momentums_price_id",
                        column: x => x.id,
                        principalTable: "price",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "price_other_indicators",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false),
                    fcb_upper = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    fcb_lower = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    he = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    pivot_highpoint = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    pivot_lowpoint = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    pivot_highline = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    pivot_lowline = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    pivot_hightrend = table.Column<int>(type: "integer", nullable: false),
                    pivot_lowtrend = table.Column<int>(type: "integer", nullable: false),
                    pp_r1 = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    pp_r2 = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    pp_r3 = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    pp_pp = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    pp_s1 = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    pp_s2 = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    pp_s3 = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    prs_prs = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    prs_sma = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    prs_percent = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    po_pbo = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    po_signal = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    rpp_r1 = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    rpp_r2 = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    rpp_r3 = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    rpp_pp = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    rpp_s1 = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    rpp_s2 = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    rpp_s3 = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    wf_bear = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    wf_bull = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    vhf_ppo = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    vhf_signal = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_price_other_indicators", x => x.id);
                    table.ForeignKey(
                        name: "fk_price_other_indicators_price_id",
                        column: x => x.id,
                        principalTable: "price",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "price_peak",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false),
                    highest_high = table.Column<bool>(type: "boolean", nullable: false),
                    offset_prev_hh = table.Column<int>(type: "integer", nullable: true),
                    offset_next_hh = table.Column<int>(type: "integer", nullable: true),
                    lowest_low = table.Column<bool>(type: "boolean", nullable: false),
                    offset_prev_ll = table.Column<int>(type: "integer", nullable: true),
                    offset_next_ll = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_price_peak", x => x.id);
                    table.ForeignKey(
                        name: "fk_price_peak_price_id",
                        column: x => x.id,
                        principalTable: "price",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "price_return",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false),
                    day_return = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    day_interval = table.Column<int>(type: "integer", nullable: true),
                    week_return = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    week_interval = table.Column<int>(type: "integer", nullable: true),
                    month_return = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    month_interval = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_price_return", x => x.id);
                    table.ForeignKey(
                        name: "fk_price_return_price_id",
                        column: x => x.id,
                        principalTable: "price",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "price_trends",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false),
                    slope_high6 = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    slope_high8 = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    slope_high12 = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    slope_high24 = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    slope_low6 = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    slope_low8 = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    slope_low12 = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    slope_low24 = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    aroon_up = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    aroon_down = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    aroon_osc = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    adx_pdi = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    adx_mdi = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    adx_adx = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    adx_adxr = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    atr_stop = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    atr_buy = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    atr_sell = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    ce_short = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    ce_long = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    hilb_dcp = table.Column<int>(type: "integer", nullable: true),
                    hilb_trend = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    hilb_smooth = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    ic_signal = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    ic_base = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    ic_leada = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    ic_leadb = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    ic_lag = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    mgd_h6 = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    mgd_h12 = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    mgd_h24 = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    mgd_h168 = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    mae_upper = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    mae_center = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    mae_lower = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    par_sar = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    par_rev = table.Column<bool>(type: "boolean", nullable: false),
                    st_combined = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    st_upper = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    st_lower = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    vi_pvi = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    vi_nvi = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    wa_jaw = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    wa_teeth = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    wa_lips = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_price_trends", x => x.id);
                    table.ForeignKey(
                        name: "fk_price_trends_price_id",
                        column: x => x.id,
                        principalTable: "price",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "price_volatilities",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false),
                    atr12_tr = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    atr12_atr = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    atr12_atrp = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    atr24_tr = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    atr24_atr = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    atr24_atrp = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    atr168_tr = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    atr168_atr = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    atr168_atrp = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    bb_upper = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    bb_lower = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    bb_percentb = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    bb_zscore = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    bb_width = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    chop = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    dc_upper = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    dc_center = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    dc_lower = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    dc_width = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    hv = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    kc_upper = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    kc_center = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    kc_lower = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    kc_width = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    sdc_upper = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    sdc_center = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    sdc_lower = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    sdc_bp = table.Column<bool>(type: "boolean", nullable: false),
                    starc_upper = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    starc_center = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    starc_lower = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    ui = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    vstop_sar = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    vstop_stop = table.Column<bool>(type: "boolean", nullable: false),
                    vstop_upper = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    vstop_lower = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_price_volatilities", x => x.id);
                    table.ForeignKey(
                        name: "fk_price_volatilities_price_id",
                        column: x => x.id,
                        principalTable: "price",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "price_volumes",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false),
                    adl_mfm = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    adl_mfv = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    adl_adl = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    adl_sma = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    cmf_mfm = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    cmf_mfv = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    cmf_cmf = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    co_mfm = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    co_mfv = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    co_adl = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    co_osc = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    fi = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    kvo_osc = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    kvo_signal = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    mfi = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    pvo_pvo = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    pvo_signal = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true),
                    pvo_hist = table.Column<decimal>(type: "numeric(35,15)", precision: 35, scale: 15, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_price_volumes", x => x.id);
                    table.ForeignKey(
                        name: "fk_price_volumes_price_id",
                        column: x => x.id,
                        principalTable: "price",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_price_crypto_id",
                table: "price",
                column: "crypto_id");

            migrationBuilder.CreateIndex(
                name: "ix_price_prediction_crypto_id",
                table: "price_prediction",
                column: "crypto_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "config");

            migrationBuilder.DropTable(
                name: "crypto_statistics");

            migrationBuilder.DropTable(
                name: "order");

            migrationBuilder.DropTable(
                name: "price_candle_sticks");

            migrationBuilder.DropTable(
                name: "price_cycles");

            migrationBuilder.DropTable(
                name: "price_ma");

            migrationBuilder.DropTable(
                name: "price_momentums");

            migrationBuilder.DropTable(
                name: "price_other_indicators");

            migrationBuilder.DropTable(
                name: "price_peak");

            migrationBuilder.DropTable(
                name: "price_prediction");

            migrationBuilder.DropTable(
                name: "price_return");

            migrationBuilder.DropTable(
                name: "price_trends");

            migrationBuilder.DropTable(
                name: "price_volatilities");

            migrationBuilder.DropTable(
                name: "price_volumes");

            migrationBuilder.DropTable(
                name: "price");

            migrationBuilder.DropTable(
                name: "crypto");
        }
    }
}
