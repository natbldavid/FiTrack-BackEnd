using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace FiTrack.Api.Migrations
{
    /// <inheritdoc />
    public partial class InitialPostgres : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "lkp_activity_types",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    icon = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_lkp_activity_types", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "lkp_exercise_catalog",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    body_part = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    exercise_type = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    exercise_demo_gif = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    is_active = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_lkp_exercise_catalog", x => x.id);
                    table.CheckConstraint("CK_lkp_exercise_catalog_body_part", "body_part IS NULL OR body_part IN ('Chest', 'Shoulders', 'Bicep', 'Tricep', 'Back', 'Quadriceps', 'Hamstring', 'Calf', 'Abs', 'Forearm')");
                    table.CheckConstraint("CK_lkp_exercise_catalog_exercise_type", "exercise_type IN ('Compound', 'Accessory', 'Core', 'Cardio')");
                });

            migrationBuilder.CreateTable(
                name: "tbl_users",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    username = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    passcode_hash = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_users", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "tbl_activity_logs",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<int>(type: "integer", nullable: false),
                    activity_type_id = table.Column<int>(type: "integer", nullable: false),
                    log_date = table.Column<DateOnly>(type: "date", nullable: false),
                    duration_minutes = table.Column<int>(type: "integer", nullable: false),
                    distance = table.Column<decimal>(type: "numeric(8,2)", precision: 8, scale: 2, nullable: true),
                    calories_burned = table.Column<int>(type: "integer", nullable: true),
                    notes = table.Column<string>(type: "text", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_activity_logs", x => x.id);
                    table.CheckConstraint("CK_tbl_activity_logs_duration", "duration_minutes > 0");
                    table.ForeignKey(
                        name: "FK_tbl_activity_logs_lkp_activity_types_activity_type_id",
                        column: x => x.activity_type_id,
                        principalTable: "lkp_activity_types",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_tbl_activity_logs_tbl_users_user_id",
                        column: x => x.user_id,
                        principalTable: "tbl_users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tbl_foods",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<int>(type: "integer", nullable: false),
                    name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    serving_description = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    image_url = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    calories = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    protein = table.Column<decimal>(type: "numeric(6,2)", precision: 6, scale: 2, nullable: false, defaultValue: 0m),
                    carbs = table.Column<decimal>(type: "numeric(6,2)", precision: 6, scale: 2, nullable: false, defaultValue: 0m),
                    fat = table.Column<decimal>(type: "numeric(6,2)", precision: 6, scale: 2, nullable: false, defaultValue: 0m),
                    is_favorite = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_foods", x => x.id);
                    table.ForeignKey(
                        name: "FK_tbl_foods_tbl_users_user_id",
                        column: x => x.user_id,
                        principalTable: "tbl_users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tbl_meals",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<int>(type: "integer", nullable: false),
                    name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    image_url = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    is_favorite = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_meals", x => x.id);
                    table.ForeignKey(
                        name: "FK_tbl_meals_tbl_users_user_id",
                        column: x => x.user_id,
                        principalTable: "tbl_users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tbl_user_goals_history",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<int>(type: "integer", nullable: false),
                    effective_from = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    effective_to = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    daily_calorie_goal = table.Column<int>(type: "integer", nullable: true),
                    daily_protein_goal = table.Column<int>(type: "integer", nullable: true),
                    daily_carb_goal = table.Column<int>(type: "integer", nullable: true),
                    daily_fat_goal = table.Column<int>(type: "integer", nullable: true),
                    weight_goal = table.Column<decimal>(type: "numeric(5,2)", precision: 5, scale: 2, nullable: true),
                    weekly_exercise_goal = table.Column<int>(type: "integer", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_user_goals_history", x => x.id);
                    table.ForeignKey(
                        name: "FK_tbl_user_goals_history_tbl_users_user_id",
                        column: x => x.user_id,
                        principalTable: "tbl_users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tbl_user_profile",
                columns: table => new
                {
                    user_id = table.Column<int>(type: "integer", nullable: false),
                    current_weight = table.Column<decimal>(type: "numeric(5,2)", precision: 5, scale: 2, nullable: true),
                    daily_calorie_goal = table.Column<int>(type: "integer", nullable: true),
                    daily_protein_goal = table.Column<int>(type: "integer", nullable: true),
                    daily_carb_goal = table.Column<int>(type: "integer", nullable: true),
                    daily_fat_goal = table.Column<int>(type: "integer", nullable: true),
                    weight_goal = table.Column<decimal>(type: "numeric(5,2)", precision: 5, scale: 2, nullable: true),
                    weekly_exercise_goal = table.Column<int>(type: "integer", nullable: true),
                    weekly_gym_goal = table.Column<int>(type: "integer", nullable: true),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_user_profile", x => x.user_id);
                    table.ForeignKey(
                        name: "FK_tbl_user_profile_tbl_users_user_id",
                        column: x => x.user_id,
                        principalTable: "tbl_users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tbl_weight_logs",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<int>(type: "integer", nullable: false),
                    log_date = table.Column<DateOnly>(type: "date", nullable: false),
                    logged_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    weight = table.Column<decimal>(type: "numeric(5,2)", precision: 5, scale: 2, nullable: false),
                    note = table.Column<string>(type: "text", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_weight_logs", x => x.id);
                    table.ForeignKey(
                        name: "FK_tbl_weight_logs_tbl_users_user_id",
                        column: x => x.user_id,
                        principalTable: "tbl_users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tbl_workout_days",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<int>(type: "integer", nullable: false),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    muscle_focus = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    sort_order = table.Column<int>(type: "integer", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_workout_days", x => x.id);
                    table.ForeignKey(
                        name: "FK_tbl_workout_days_tbl_users_user_id",
                        column: x => x.user_id,
                        principalTable: "tbl_users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tbl_food_logs",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<int>(type: "integer", nullable: false),
                    log_date = table.Column<DateOnly>(type: "date", nullable: false),
                    logged_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    source_type = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    food_id = table.Column<int>(type: "integer", nullable: true),
                    meal_id = table.Column<int>(type: "integer", nullable: true),
                    name_snapshot = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    serving_description_snapshot = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    calories = table.Column<int>(type: "integer", nullable: false),
                    protein = table.Column<decimal>(type: "numeric(6,2)", precision: 6, scale: 2, nullable: false),
                    carbs = table.Column<decimal>(type: "numeric(6,2)", precision: 6, scale: 2, nullable: false),
                    fat = table.Column<decimal>(type: "numeric(6,2)", precision: 6, scale: 2, nullable: false),
                    quantity = table.Column<decimal>(type: "numeric(8,2)", precision: 8, scale: 2, nullable: false, defaultValue: 1m),
                    meal_slot = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    note = table.Column<string>(type: "text", nullable: true),
                    is_active = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_food_logs", x => x.id);
                    table.CheckConstraint("CK_tbl_food_logs_meal_slot", "meal_slot IS NULL OR meal_slot IN ('breakfast', 'lunch', 'dinner', 'snack')");
                    table.CheckConstraint("CK_tbl_food_logs_source_type", "source_type IN ('food', 'meal', 'custom')");
                    table.ForeignKey(
                        name: "FK_tbl_food_logs_tbl_foods_food_id",
                        column: x => x.food_id,
                        principalTable: "tbl_foods",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_tbl_food_logs_tbl_meals_meal_id",
                        column: x => x.meal_id,
                        principalTable: "tbl_meals",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_tbl_food_logs_tbl_users_user_id",
                        column: x => x.user_id,
                        principalTable: "tbl_users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tbl_meal_items",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    meal_id = table.Column<int>(type: "integer", nullable: false),
                    food_id = table.Column<int>(type: "integer", nullable: false),
                    quantity = table.Column<decimal>(type: "numeric(8,2)", precision: 8, scale: 2, nullable: false, defaultValue: 1m),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_meal_items", x => x.id);
                    table.ForeignKey(
                        name: "FK_tbl_meal_items_tbl_foods_food_id",
                        column: x => x.food_id,
                        principalTable: "tbl_foods",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbl_meal_items_tbl_meals_meal_id",
                        column: x => x.meal_id,
                        principalTable: "tbl_meals",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tbl_workout_day_exercises",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    workout_day_id = table.Column<int>(type: "integer", nullable: false),
                    exercise_id = table.Column<int>(type: "integer", nullable: false),
                    exercise_order = table.Column<int>(type: "integer", nullable: false),
                    target_sets = table.Column<int>(type: "integer", nullable: false),
                    target_reps_min = table.Column<int>(type: "integer", nullable: false),
                    target_reps_max = table.Column<int>(type: "integer", nullable: false),
                    initial_weight = table.Column<decimal>(type: "numeric(6,2)", precision: 6, scale: 2, nullable: true),
                    current_working_weight = table.Column<decimal>(type: "numeric(6,2)", precision: 6, scale: 2, nullable: true),
                    notes = table.Column<string>(type: "text", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_workout_day_exercises", x => x.id);
                    table.CheckConstraint("CK_tbl_workout_day_exercises_exercise_order", "exercise_order > 0");
                    table.CheckConstraint("CK_tbl_workout_day_exercises_target_reps_max", "target_reps_max > 0");
                    table.CheckConstraint("CK_tbl_workout_day_exercises_target_reps_min", "target_reps_min > 0");
                    table.CheckConstraint("CK_tbl_workout_day_exercises_target_reps_range", "target_reps_max >= target_reps_min");
                    table.CheckConstraint("CK_tbl_workout_day_exercises_target_sets", "target_sets > 0");
                    table.ForeignKey(
                        name: "FK_tbl_workout_day_exercises_lkp_exercise_catalog_exercise_id",
                        column: x => x.exercise_id,
                        principalTable: "lkp_exercise_catalog",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbl_workout_day_exercises_tbl_workout_days_workout_day_id",
                        column: x => x.workout_day_id,
                        principalTable: "tbl_workout_days",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tbl_workout_sessions",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<int>(type: "integer", nullable: false),
                    workout_day_id = table.Column<int>(type: "integer", nullable: true),
                    session_name = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    session_date = table.Column<DateOnly>(type: "date", nullable: false),
                    started_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    completed_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    notes = table.Column<string>(type: "text", nullable: true),
                    is_active = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_workout_sessions", x => x.id);
                    table.CheckConstraint("CK_tbl_workout_sessions_status", "status IN ('in_progress', 'completed', 'cancelled')");
                    table.ForeignKey(
                        name: "FK_tbl_workout_sessions_tbl_users_user_id",
                        column: x => x.user_id,
                        principalTable: "tbl_users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tbl_workout_sessions_tbl_workout_days_workout_day_id",
                        column: x => x.workout_day_id,
                        principalTable: "tbl_workout_days",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "tbl_workout_session_exercises",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    workout_session_id = table.Column<int>(type: "integer", nullable: false),
                    exercise_id = table.Column<int>(type: "integer", nullable: false),
                    exercise_name_snapshot = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    body_part_snapshot = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    exercise_type_snapshot = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    exercise_order = table.Column<int>(type: "integer", nullable: false),
                    target_sets = table.Column<int>(type: "integer", nullable: true),
                    target_reps_min = table.Column<int>(type: "integer", nullable: true),
                    target_reps_max = table.Column<int>(type: "integer", nullable: true),
                    planned_working_weight = table.Column<decimal>(type: "numeric(6,2)", precision: 6, scale: 2, nullable: true),
                    actual_working_weight = table.Column<decimal>(type: "numeric(6,2)", precision: 6, scale: 2, nullable: true),
                    notes = table.Column<string>(type: "text", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_workout_session_exercises", x => x.id);
                    table.CheckConstraint("CK_tbl_workout_session_exercises_exercise_order", "exercise_order > 0");
                    table.CheckConstraint("CK_tbl_workout_session_exercises_target_reps_max", "target_reps_max IS NULL OR target_reps_max > 0");
                    table.CheckConstraint("CK_tbl_workout_session_exercises_target_reps_min", "target_reps_min IS NULL OR target_reps_min > 0");
                    table.CheckConstraint("CK_tbl_workout_session_exercises_target_reps_range", "(target_reps_min IS NULL AND target_reps_max IS NULL) OR (target_reps_min IS NOT NULL AND target_reps_max IS NOT NULL AND target_reps_max >= target_reps_min)");
                    table.CheckConstraint("CK_tbl_workout_session_exercises_target_sets", "target_sets IS NULL OR target_sets > 0");
                    table.ForeignKey(
                        name: "FK_tbl_workout_session_exercises_lkp_exercise_catalog_exercise~",
                        column: x => x.exercise_id,
                        principalTable: "lkp_exercise_catalog",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_tbl_workout_session_exercises_tbl_workout_sessions_workout_~",
                        column: x => x.workout_session_id,
                        principalTable: "tbl_workout_sessions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tbl_workout_set_logs",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    workout_session_exercise_id = table.Column<int>(type: "integer", nullable: false),
                    set_number = table.Column<int>(type: "integer", nullable: false),
                    reps = table.Column<int>(type: "integer", nullable: false),
                    weight = table.Column<decimal>(type: "numeric(6,2)", precision: 6, scale: 2, nullable: true),
                    completed = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_workout_set_logs", x => x.id);
                    table.CheckConstraint("CK_tbl_workout_set_logs_reps", "reps >= 0");
                    table.CheckConstraint("CK_tbl_workout_set_logs_set_number", "set_number > 0");
                    table.ForeignKey(
                        name: "FK_tbl_workout_set_logs_tbl_workout_session_exercises_workout_~",
                        column: x => x.workout_session_exercise_id,
                        principalTable: "tbl_workout_session_exercises",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "tbl_users",
                columns: new[] { "id", "created_at", "passcode_hash", "updated_at", "username" },
                values: new object[] { 1, new DateTime(2026, 3, 8, 0, 0, 0, 0, DateTimeKind.Utc), "AQAAAAIAAYagAAAAEMOCK8GkM8J7W0X0dM5mA3O3Nn7Q3qV2Pq1r0J9G4k8x6mVh2Qq8qP1p4L5j9A==", new DateTime(2026, 3, 8, 0, 0, 0, 0, DateTimeKind.Utc), "demo_user" });

            migrationBuilder.CreateIndex(
                name: "IX_lkp_activity_types_name",
                table: "lkp_activity_types",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_lkp_exercise_catalog_body_part",
                table: "lkp_exercise_catalog",
                column: "body_part");

            migrationBuilder.CreateIndex(
                name: "IX_lkp_exercise_catalog_exercise_type",
                table: "lkp_exercise_catalog",
                column: "exercise_type");

            migrationBuilder.CreateIndex(
                name: "IX_lkp_exercise_catalog_is_active",
                table: "lkp_exercise_catalog",
                column: "is_active");

            migrationBuilder.CreateIndex(
                name: "IX_lkp_exercise_catalog_name",
                table: "lkp_exercise_catalog",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_tbl_activity_logs_activity_type_id",
                table: "tbl_activity_logs",
                column: "activity_type_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_activity_logs_user_id",
                table: "tbl_activity_logs",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_activity_logs_user_id_log_date",
                table: "tbl_activity_logs",
                columns: new[] { "user_id", "log_date" });

            migrationBuilder.CreateIndex(
                name: "IX_tbl_food_logs_food_id",
                table: "tbl_food_logs",
                column: "food_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_food_logs_meal_id",
                table: "tbl_food_logs",
                column: "meal_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_food_logs_user_id_log_date",
                table: "tbl_food_logs",
                columns: new[] { "user_id", "log_date" });

            migrationBuilder.CreateIndex(
                name: "IX_tbl_food_logs_user_id_logged_at",
                table: "tbl_food_logs",
                columns: new[] { "user_id", "logged_at" });

            migrationBuilder.CreateIndex(
                name: "IX_tbl_foods_user_id",
                table: "tbl_foods",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_foods_user_id_name",
                table: "tbl_foods",
                columns: new[] { "user_id", "name" });

            migrationBuilder.CreateIndex(
                name: "IX_tbl_meal_items_food_id",
                table: "tbl_meal_items",
                column: "food_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_meal_items_meal_id",
                table: "tbl_meal_items",
                column: "meal_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_meal_items_meal_id_food_id",
                table: "tbl_meal_items",
                columns: new[] { "meal_id", "food_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_tbl_meals_user_id",
                table: "tbl_meals",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_meals_user_id_name",
                table: "tbl_meals",
                columns: new[] { "user_id", "name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_tbl_user_goals_history_user_id",
                table: "tbl_user_goals_history",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_user_goals_history_user_id_effective_from",
                table: "tbl_user_goals_history",
                columns: new[] { "user_id", "effective_from" });

            migrationBuilder.CreateIndex(
                name: "IX_tbl_users_username",
                table: "tbl_users",
                column: "username",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_tbl_weight_logs_user_id_log_date",
                table: "tbl_weight_logs",
                columns: new[] { "user_id", "log_date" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_tbl_workout_day_exercises_exercise_id",
                table: "tbl_workout_day_exercises",
                column: "exercise_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_workout_day_exercises_workout_day_id",
                table: "tbl_workout_day_exercises",
                column: "workout_day_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_workout_day_exercises_workout_day_id_exercise_order",
                table: "tbl_workout_day_exercises",
                columns: new[] { "workout_day_id", "exercise_order" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_tbl_workout_days_user_id",
                table: "tbl_workout_days",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_workout_days_user_id_name",
                table: "tbl_workout_days",
                columns: new[] { "user_id", "name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_tbl_workout_days_user_id_sort_order",
                table: "tbl_workout_days",
                columns: new[] { "user_id", "sort_order" });

            migrationBuilder.CreateIndex(
                name: "IX_tbl_workout_session_exercises_exercise_id",
                table: "tbl_workout_session_exercises",
                column: "exercise_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_workout_session_exercises_workout_session_id",
                table: "tbl_workout_session_exercises",
                column: "workout_session_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_workout_session_exercises_workout_session_id_exercise_o~",
                table: "tbl_workout_session_exercises",
                columns: new[] { "workout_session_id", "exercise_order" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_tbl_workout_sessions_status",
                table: "tbl_workout_sessions",
                column: "status");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_workout_sessions_user_id",
                table: "tbl_workout_sessions",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_workout_sessions_user_id_is_active",
                table: "tbl_workout_sessions",
                columns: new[] { "user_id", "is_active" });

            migrationBuilder.CreateIndex(
                name: "IX_tbl_workout_sessions_user_id_session_date",
                table: "tbl_workout_sessions",
                columns: new[] { "user_id", "session_date" });

            migrationBuilder.CreateIndex(
                name: "IX_tbl_workout_sessions_user_id_started_at",
                table: "tbl_workout_sessions",
                columns: new[] { "user_id", "started_at" });

            migrationBuilder.CreateIndex(
                name: "IX_tbl_workout_sessions_workout_day_id",
                table: "tbl_workout_sessions",
                column: "workout_day_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_workout_set_logs_workout_session_exercise_id",
                table: "tbl_workout_set_logs",
                column: "workout_session_exercise_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_workout_set_logs_workout_session_exercise_id_set_number",
                table: "tbl_workout_set_logs",
                columns: new[] { "workout_session_exercise_id", "set_number" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tbl_activity_logs");

            migrationBuilder.DropTable(
                name: "tbl_food_logs");

            migrationBuilder.DropTable(
                name: "tbl_meal_items");

            migrationBuilder.DropTable(
                name: "tbl_user_goals_history");

            migrationBuilder.DropTable(
                name: "tbl_user_profile");

            migrationBuilder.DropTable(
                name: "tbl_weight_logs");

            migrationBuilder.DropTable(
                name: "tbl_workout_day_exercises");

            migrationBuilder.DropTable(
                name: "tbl_workout_set_logs");

            migrationBuilder.DropTable(
                name: "lkp_activity_types");

            migrationBuilder.DropTable(
                name: "tbl_foods");

            migrationBuilder.DropTable(
                name: "tbl_meals");

            migrationBuilder.DropTable(
                name: "tbl_workout_session_exercises");

            migrationBuilder.DropTable(
                name: "lkp_exercise_catalog");

            migrationBuilder.DropTable(
                name: "tbl_workout_sessions");

            migrationBuilder.DropTable(
                name: "tbl_workout_days");

            migrationBuilder.DropTable(
                name: "tbl_users");
        }
    }
}
