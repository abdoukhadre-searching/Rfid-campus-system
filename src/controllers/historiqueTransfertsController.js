const DBConnection = require ("./../configs/DBConnection")

const getPagehistroriqueTransfert = (req, res) => {
    
        const sql1 = `SELECT * FROM transferts WHERE codePermanentSource = ${req.user.codePermanent}`
        DBConnection.query(sql1, (err, result) => {
            if (err) {
                return res.status(500).send(err)
            }                             
            res.render("historique", { 
                resultat: result
            })            
        })
}

module.exports = {
    getPagehistroriqueTransfert: getPagehistroriqueTransfert
}