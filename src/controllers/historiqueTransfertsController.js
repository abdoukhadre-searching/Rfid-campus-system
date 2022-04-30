import  {connection} from "./../configs/DBConnection.js"

export const getPagehistroriqueTransfert = (req, res) => {
    
        const sql1 = `SELECT * FROM transferts WHERE codePermanentSource = ${req.user.codePermanent}`
        connection.query(sql1, (err, result) => {
            if (err) {
                return res.status(500).send(err)
            }                             
            res.render("historique", { 
                resultat: result
            })            
        })
}

// module.exports = {
//     getPagehistroriqueTransfert: getPagehistroriqueTransfert
// }