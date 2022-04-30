// const DBConnection = require ("./../configs/DBConnection")
import {connection} from "./../configs/DBConnection.js"
export const handleHelloWorld = (req, res) => {
    console.log(req.user)
    
        const sql = `SELECT * FROM etudiant WHERE codePermanent = ${req.user.codePermanent}`
        connection.query(sql, (err, data) => {
            if (err) {
                return res.status(500).send(err)
            }
            res.render("homepage", {
                async: false, // ajouter a cause des 'if else' effectuer au frontend sur les données reçues(false au depart)
                etudiant: data[0]
            })
            console.log(data)
        })
}

export const activerDesactiverCarte = (req, res) =>{

        const sql2 = `SELECT * FROM etudiant WHERE codePermanent = ${req.user.codePermanent}`
        connection.query(sql2, (err, result) => {
            if (err) {
                return res.status(500).send(err)
            }
            if (result[0].etatCarte == 0) {
                const query = `UPDATE etudiant SET etatCarte = 1 WHERE codePermanent = ${(result[0].codePermanent)}`
                connection.query(query , (err, _data) => {
                    if (err) {
                        return res.status(500).send(err)
                    } 
                })         
            } else {
                const query2 = `UPDATE etudiant SET etatCarte = 0 WHERE codePermanent = ${(result[0].codePermanent)} `
                connection.query (query2 , (err, data) => {
                    if (err) {
                        return res.status(500).send(err)
                    } 
                })
            }
            res.redirect("/acceuil")
        })
}

// module.exports = {
//     handleHelloWorld: handleHelloWorld,
//     activerDesactiverCarte: activerDesactiverCarte
// }