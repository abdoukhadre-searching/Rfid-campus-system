const DBConnection = require ("./../configs/DBConnection")

const getPagehistroriqueTransfert = (req, res) => {

    const id = req.user.id
    const sql = `SELECT (codePermanent) FROM compte WHERE id = ${id}`

    DBConnection.query(sql, (err, data)=>{
        if (err) {
            return res.status(500).send(err)
        }
        const sql1 = `SELECT * FROM transactions WHERE codeSource = ${data[0].codePermanent}`
        DBConnection.query(sql1, (err, result) => {
            if (err) {
                return res.status(500).send(err)
            }                             
            res.render("historique", { 
                resultat: result
            })            
        })
    })
}


module.exports = {
    //historiqueTransfert: historiqueTransfert,
    getPagehistroriqueTransfert: getPagehistroriqueTransfert
}