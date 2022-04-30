import express from "express";
/*
 * Config view engine for app
 */
export const configViewEngine = (app)=> {
    app.use(express.static("./src/public"));
    app.set("view engine", "ejs");
    app.set("views","./src/views");
};

// module.exports = configViewEngine;
