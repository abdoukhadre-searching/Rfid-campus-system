import 'dotenv/config'
import mysql from "mysql2"

export const connection = mysql.createConnection({
    host: process.env.DB_HOST,
    user: process.env.DB_USERNAME,
    password: process.env.DB_PASSWORD,
    database: process.env.DB_NAME
})

connection.connect((err) => {
    if (err) throw err
    console.log("Connection à la base de données ! Etablie ")
})

// module.exports = connection