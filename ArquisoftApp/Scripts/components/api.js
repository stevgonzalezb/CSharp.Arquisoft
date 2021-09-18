
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

}