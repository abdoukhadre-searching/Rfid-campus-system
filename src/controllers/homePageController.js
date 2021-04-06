const DBConnection = require ("./../configs/DBConnection")

const handleHelloWorld = (req, res) => {

    const id= req.user.id
    const sql = `SELECT (codePermanent) FROM compte WHERE id = ${id}`

    DBConnection.query(sql, (err, resultat)=>{
        if (err) {
            return res.status(500).send(err)
        }
        console.log(resultat[0].codePermanent)

        const sql2 = `SELECT * FROM etudiants WHERE codePermanent = ${resultat[0].codePermanent}`
        DBConnection.query(sql2, (err, data) => {
            if (err) {
                return res.status(500).send(err)
            }
            res.render("homepage", {
                //user: req.user,
                async: false, // a cause des 'if else' effectuer au frontend sur les données reçues(false au depart)
                etudiant: data[0]
            })
            console.log(data)
        })
        
    })
}

const activerDesactiverCarte = (req, res) =>{

    const id= req.user.id
    const sql = `SELECT (codePermanent) FROM compte WHERE id = ${id}`

    DBConnection.query(sql, (err, data)=>{
        if (err) {
            return res.status(500).send(err)
        }
        const sql2 = `SELECT * FROM etudiants WHERE codePermanent = ${data[0].codePermanent}`
        DBConnection.query(sql2, (err, result) => {
            if (err) {
                return res.status(500).send(err)
            }
            if (result[0].statusCarte == 0) {
                const query = `UPDATE etudiants SET statusCarte = 1 WHERE codePermanent = ${(result[0].codePermanent)}`
                DBConnection.query(query , (err, data) => {
                    if (err) {
                        return res.status(500).send(err)
                    } 
                })         
            } else {
                const query2 = `UPDATE etudiants SET statusCarte = 0 WHERE codePermanent = ${(result[0].codePermanent)} `
                DBConnection.query (query2 , (err, data) => {
                    if (err) {
                        return res.status(500).send(err)
                    } 
                })
            }
            res.redirect("/acceuil")
        })
    })
}

module.exports = {
    handleHelloWorld: handleHelloWorld,
    activerDesactiverCarte: activerDesactiverCarte
}