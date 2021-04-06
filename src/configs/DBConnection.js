require('dotenv').config()
import mysql from "mysql2"

let connection = mysql.createConnection({
    host: process.env.DB_HOST,
    user: process.env.DB_USERNAME,
    password: process.env.DB_PASSWORD,
    database: process.env.DB_NAME
})

connection.connect(function(err) {
    if (err) throw err
    console.log("Connection à la base de données ! Etablie ")
})

module.exports = connection