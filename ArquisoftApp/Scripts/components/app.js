
var app = app || {};

app.enums = {
    Regex: {
        Email: /^(([^<>()[\]\\.,;:\s@"]+(\.[^<>()[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/,
        Password: /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)[a-zA-Z\d]{8,}$/
    },

    States: {
        ACTIVE: 1,
        DISABLE: 2,
        DELETE: 3
    },

    Modules: {
        CLIENTS: 1,
        MATERIALS: 2,
        PROJECTS: 3,
        VENDOR_MATERIALS: 4
    },

    Permissions: {
        //Clients
        CLIENT_READ: 1,
        CLIENT_EDIT: 2,
        CLIENT_DELETE: 3,
        CLIENT_ADD: 4,

        //Materials
        MATERIAL_READ: 5,
        MATERIAL_EDIT: 6,
        MATERIAL_DELETE: 7,
        MATERIAL_ADD: 8,

        //Projects
        PROJECT_READ: 9,
        PROJECT_EDIT: 10,
        PROJECT_DELETE: 11,
        PROJECT_ADD: 12,

        //VendorMaterials
        VENDOR_MAT_READ: 13,
        VENDOR_MAT_RUN: 14
        
    }
}

