using System.Collections.ObjectModel;

namespace VetClinic.Core.Permissions
{
    public static class ApplicationPermissions
    {
        public static ReadOnlyCollection<ApplicationPermission> AllPermissions;

        public const string PetOwnerPermissionGroupName = "Pet Owner Permissions";
        public static ApplicationPermission ViewPetOwners = new ApplicationPermission("View PetOwners", "petowners.view", PetOwnerPermissionGroupName, "Permission to view other Pet Owner details");
        public static ApplicationPermission ManagePetOwners = new ApplicationPermission("Manage PetOwners", "petowners.manage", PetOwnerPermissionGroupName, "Permission to create, delete and modify other pet owner details");

        public const string VetPermissionGroupName = "Vet Permissions";
        public static ApplicationPermission ViewVets = new ApplicationPermission("View Vets", "vets.view", VetPermissionGroupName, "Permission to view other Vet details");
        public static ApplicationPermission ManageVets = new ApplicationPermission("Manage Vets", "vets.manage", VetPermissionGroupName, "Permission to create, delete and modify other Vet details");

        public const string PetDetailPermissionGroupName = "Pet Detail Permissions";
        public static ApplicationPermission ViewPetDetails = new ApplicationPermission("View Pet Details", "petdetails.view", PetDetailPermissionGroupName, "Permission to view other Pet details");
        public static ApplicationPermission ManagePetDetails = new ApplicationPermission("Manage Pet Details", "petdetails.manage", PetDetailPermissionGroupName, "Permission to create, delete and modify other Pet details");

        public const string VisitPermissionGroupName = "Visit Permissions";
        public static ApplicationPermission ViewVisits = new ApplicationPermission("View Visits", "petdetails.view", VisitPermissionGroupName, "Permission to view Visits");
        public static ApplicationPermission ManageVisits = new ApplicationPermission("Manage Visits", "petdetails.manage", VisitPermissionGroupName, "Permission to create, delete and modify Visits");

        public const string UsersPermissionGroupName = "User Permissions";
        public static ApplicationPermission ViewUsers = new ApplicationPermission("View Users", "users.view", UsersPermissionGroupName, "Permission to view other users account details");
        public static ApplicationPermission ManageUsers = new ApplicationPermission("Manage Users", "users.manage", UsersPermissionGroupName, "Permission to create, delete and modify other users account details");

        public const string RolesPermissionGroupName = "Role Permissions";
        public static ApplicationPermission ViewRoles = new ApplicationPermission("View Roles", "roles.view", RolesPermissionGroupName, "Permission to view available roles");
        public static ApplicationPermission ManageRoles = new ApplicationPermission("Manage Roles", "roles.manage", RolesPermissionGroupName, "Permission to create, delete and modify roles");
        public static ApplicationPermission AssignRoles = new ApplicationPermission("Assign Roles", "roles.assign", RolesPermissionGroupName, "Permission to assign roles to users");


        static ApplicationPermissions()
        {
            List<ApplicationPermission> allPermissions = new List<ApplicationPermission>()
            {
                ViewUsers,
                ManageUsers,

                ViewPetOwners,
                ManagePetOwners,

                ViewVets,
                ManageVets,

                ViewRoles,
                ManageRoles,
                AssignRoles
            };

            AllPermissions = allPermissions.AsReadOnly();
        }

        public static ApplicationPermission GetPermissionByName(string permissionName)
        {
            return AllPermissions.Where(p => p.Name == permissionName).SingleOrDefault();
        }

        public static ApplicationPermission GetPermissionByValue(string permissionValue)
        {
            return AllPermissions.Where(p => p.Value == permissionValue).SingleOrDefault();
        }

        public static string[] GetAllPermissionValues()
        {
            return AllPermissions.Select(p => p.Value).ToArray();
        }

        public static string[] GetAdministrativePermissionValues()
        {
            return new string[] { ManageUsers, ManageRoles, AssignRoles, ManagePetOwners, ManageVets };
        }
    }
}
