const DBConnection = require ("./../configs/DBConnection")

const achatHistorique = (req, res) => {
            const sql = `SELECT idAchats
                FROM 
                achatparcarte
                WHERE
                codePermanent = ${req.user.codePermanent}`
                DBConnection.query(sql, (err, result) => {
                    if (err) {
                        return res.status(500).send(err)
                    }
                    console.log(result)
                        const query  = `SELECT * FROM achats WHERE idAchats = ${result[1].idAchats}`
                        DBConnection.query(query, (err, data) => {
                            if (err) {
                                return res.status(500).send(err)
                            }
                            console.log(data)                              
                            return res.render('historiqueAchat', {
                            get_achat: data
                            })                      
                        })
                })
}

module.exports = {
    achatHistorique: achatHistorique
}