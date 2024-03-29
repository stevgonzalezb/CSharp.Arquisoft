﻿
var app = app || {};


//app.fomatter = {
//    FormatCurrency = new Intl.NumberFormat('en-US', {
//        style: 'currency',
//        currency: 'CRC',
//    })
//}

app.enums = {
    Regex: {
        Email: /^(([^<>()[\]\\.,;:\s@"]+(\.[^<>()[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/,
        Password: /^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-])(?!.*?([0123456789])\1).{9,}$/,
        /*Regex valida Mayusculas, minusculas, 9 caracters y simbolos, pero no números consecutivos */
        /*/^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{9,}$/,*/
        /*/^ (?=.* [a - z])(?=.* [A - Z])(?=.*\d)[a - zA - Z\d]{ 8,} $ /*/
        PhoneNumbers: /^[2|4|6-8]\d{3}-?\d{4}$/,
        OnlyNumber: /^([0-9])*$/
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

