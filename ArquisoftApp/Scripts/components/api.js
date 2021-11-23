
/** Dashboard API **/
class DashboardApi {
    // List all clients
    GetDashboardData = async function () {
        return $.ajax({
            url: `/ArquisoftApp/Home/GetDashboardData`,
            type: "GET",
            dataType: "json",
            contentType: "application/json; charset=utf-8",
        })
    }
}

/**  User Maintenance API  **/
class UserApi {

    // List all users
    ListUsers = async function () {
        return $.ajax({
            url: "/ArquisoftApp/User/List",
            type: "GET",
            dataType: "json",
            contentType: "application/json; charset=utf-8",
        });
    }
    // Get single user by Id
    GetUser = async function (userId) {
        return $.ajax({
            url: `/ArquisoftApp/User/Get?userId=${userId}`,
            type: "GET",
            dataType: "json",
            contentType: "application/json; charset=utf-8",
        });
    }
    // Save or Update user data
    SaveUser = async function (data) {
        return $.ajax({
            url: "/ArquisoftApp/User/Save",
            type: "POST",
            data: JSON.stringify(data),
            dataType: "json",
            contentType: "application/json; charset=utf-8",
        });
    }
    // Delete user - Will change the state
    DeleteUser = async function (userId) {
        return $.ajax({
            url: `/ArquisoftApp/User/Delete?userId=${userId}`,
            type: "GET",
            dataType: "json",
            contentType: "application/json; charset=utf-8",
        });
    }
    // Get current session user
    GetSessionUser = async function () {
        return $.ajax({
            url: `/ArquisoftApp/User/GetSessionUser`,
            type: "GET",
            dataType: "json",
            contentType: "application/json; charset=utf-8",
        });
    }

    SaveCurrentUser = async function (data) {
        return $.ajax({
            url: `/ArquisoftApp/User/SaveCurrentUser`,
            type: "POST",
            data: JSON.stringify(data),
            dataType: "json",
            contentType: "application/json; charset=utf-8",
        });
    }
}

/**  Role Maintenance API  **/
class RoleApi {

    // List all roles
    ListRoles = async function () {
        return $.ajax({
            url: "/ArquisoftApp/Role/List",
            type: "GET",
            dataType: "json",
            contentType: "application/json; charset=utf-8",
        });
    }
    // Get single role by Id
    GetRole = async function (roleId) {
        return $.ajax({
            url: `/ArquisoftApp/Role/Get?roleId=${roleId}`,
            type: "GET",
            dataType: "json",
            contentType: "application/json; charset=utf-8",
        });
    }
    // Save or Update Role data
    SaveRole = async function (data) {
        return $.ajax({
            url: "/ArquisoftApp/Role/Save",
            type: "POST",
            data: JSON.stringify(data),
            dataType: "json",
            contentType: "application/json; charset=utf-8",
        });
    }
    // List all permissions by role
    GetPermissionsByRole = async function (roleId) {
        return $.ajax({
            url: `/ArquisoftApp/Role/GetPermissionsByRole?roleId=${roleId}`,
            type: "GET",
            dataType: "json",
            contentType: "application/json; charset=utf-8",
        });
    }

}

/**  Clients Maintenance API  **/
class ClientApi {
    // List all clients
    ListClients = async function () {
        return $.ajax({
            url: "/ArquisoftApp/Client/List",
            type: "GET",
            dataType: "json",
            contentType: "application/json; charset=utf-8",
        });
    }

    // Get single client by Id
    GetClient = async function (clientId) {
        return $.ajax({
            url: `/ArquisoftApp/Client/Get?clientId=${clientId}`,
            type: "GET",
            dataType: "json",
            contentType: "application/json; charset=utf-8",
        });
    }

    // Save or Update Client data
    SaveClient = async function (data) {
        return $.ajax({
            url: "/ArquisoftApp/Client/Save",
            type: "POST",
            data: JSON.stringify(data),
            dataType: "json",
            contentType: "application/json; charset=utf-8",
        });
    }

    // Delete user - Will change the state
    DeleteClient = async function (clientId) {
        return $.ajax({
            url: `/ArquisoftApp/Client/Delete?clientId=${clientId}`,
            type: "GET",
            dataType: "json",
            contentType: "application/json; charset=utf-8",
        });
    }
}

class MaterialApi {
    // List all Vendor Materials
    ListVendorMaterials = async function () {
        return $.ajax({
            url: "/ArquisoftApp/Material/ListVendorMaterials",
            type: "GET",
            dataType: "json",
            contentType: "application/json; charset=utf-8",
        });
    }

    // Get single vendor material by Id
    GetVendorMaterial = async function (materialId) {
        return $.ajax({
            url: `/ArquisoftApp/Material/GetVendorMaterial?materialId=${materialId}`,
            type: "GET",
            dataType: "json",
            contentType: "application/json; charset=utf-8",
        });
    }



    // List all materials
    ListMaterials = async function () {
        return $.ajax({
            url: "/ArquisoftApp/Material/ListMaterials",
            type: "GET",
            dataType: "json",
            contentType: "application/json; charset=utf-8",
        });
    }

    // Get single material by Id
    GetMaterial = async function (materialId) {
        return $.ajax({
            url: `/ArquisoftApp/Material/GetMaterial?materialId=${materialId}`,
            type: "GET",
            dataType: "json",
            contentType: "application/json; charset=utf-8",
        });
    }

    // Save or Update material data
    SaveMaterial = async function (data) {
        return $.ajax({
            url: "/ArquisoftApp/Material/Save",
            type: "POST",
            data: JSON.stringify(data),
            dataType: "json",
            contentType: "application/json; charset=utf-8",
        });
    }

    // Delete material - Will change the state
    DeleteMaterial = async function (materialId) {
        return $.ajax({
            url: `/ArquisoftApp/Material/DeleteMaterial?materialId=${materialId}`,
            type: "GET",
            dataType: "json",
            contentType: "application/json; charset=utf-8",
        });
    }

}

/**  Projects Maintenance API  **/
class ProjectApi {
    // List all clients
    ListProjects = async function () {
        return $.ajax({
            url: "/ArquisoftApp/Project/ListProjects",
            type: "GET",
            dataType: "json",
            contentType: "application/json; charset=utf-8",
        });
    }

    // Get general data 
    GetGeneralData = async function (projectId) {
        return $.ajax({
            url: `/ArquisoftApp/Project/GetGeneralData?projectId=${projectId}`,
            type: "GET",
            dataType: "json",
            contentType: "application/json; charset=utf-8",
        });
    };

    // Save or Update Project data
    SaveProject = async function (data) {
        return $.ajax({
            url: "/ArquisoftApp/Project/Save",
            type: "POST",
            data: JSON.stringify(data),
            dataType: "json",
            contentType: "application/json; charset=utf-8",
        });
    }

    // Delete project
    DeleteProject = async function (projectId) {
        return $.ajax({
            url: `/ArquisoftApp/Project/Delete?projectId=${projectId}`,
            type: "GET",
            dataType: "json",
            contentType: "application/json; charset=utf-8",
        });
    }

    // Upload attachments
    UploadAttachment = async function (fileData) {
        return $.ajax({
            url: "/ArquisoftApp/Project/UploadAttachment",
            type: "POST",
            data: fileData,
            processData: false,  
            contentType: false 
        });
    }

    // Delete Attachment
    DeleteFile = async function (id, projectId) {
        return $.ajax({
            url: `/ArquisoftApp/Project/DeleteFile?Id=${id}&projectId=${projectId}`,
            type: "GET",
            dataType: "json",
            contentType: "application/json; charset=utf-8",
        });
    };
}

/**  Budget Maintenance API  **/
class BudgetApi {
    // List all clients
    ListBudgets = async function (projectId) {
        return $.ajax({
            url: `/ArquisoftApp/Budget/ListBudgets?projectId=${projectId}`,
            type: "GET",
            dataType: "json",
            contentType: "application/json; charset=utf-8",
        });
    }

    // Delete budget
    DeleteBudget = async function (budgetId) {
        return $.ajax({
            url: `/ArquisoftApp/Budget/Delete?budgetId=${budgetId}`,
            type: "GET",
            dataType: "json",
            contentType: "application/json; charset=utf-8",
        });
    }

    // List budget lines
    ListBudgets = async function (budgetId) {
        return $.ajax({
            url: `/ArquisoftApp/Budget/GetBudgetLines?budgetId=${budgetId}`,
            type: "GET",
            dataType: "json",
            contentType: "application/json; charset=utf-8",
        });
    }

    // Save or Update Budget
    SaveBudget = async function (data) {
        return $.ajax({
            url: "/ArquisoftApp/Budget/SaveBudget",
            type: "POST",
            data: JSON.stringify(data),
            dataType: "json",
            contentType: "application/json; charset=utf-8",
        });
    }

    // Save or Update BudgetLine data
    SaveBudgetLine = async function (data) {
        return $.ajax({
            url: "/ArquisoftApp/Budget/SaveBudgetLine",
            type: "POST",
            data: JSON.stringify(data),
            dataType: "json",
            contentType: "application/json; charset=utf-8",
        });
    }

    // Delete budget Line
    DeleteBudgetLine = async function (data) {
        return $.ajax({
            url: `/ArquisoftApp/Budget/DeleteBudgetLine`,
            type: "POST",
            data: JSON.stringify(data),
            dataType: "json",
            contentType: "application/json; charset=utf-8",
        });
    }
}

/** Settings API **/
class SettingsApi {
    // List all settings
    GetSettings = async function () {
        return $.ajax({
            url: `/ArquisoftApp/Setting/GetSettings`,
            type: "GET",
            dataType: "json",
            contentType: "application/json; charset=utf-8",
        })
    }

    SaveSettings = async function (data) {
        return $.ajax({
            url: `/ArquisoftApp/Setting/SaveSettings`,
            type: "POST",
            data: JSON.stringify(data),
            dataType: "json",
            contentType: "application/json; charset=utf-8",
        });
    }
}