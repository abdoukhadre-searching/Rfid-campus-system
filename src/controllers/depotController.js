const DBConnection = require ("./../configs/DBConnection")

const getpageTransfert = (req,res) => {
   return res.render("transfert")
}
const transfert = async (req , res) => {
    
    const date_ob = new Date(),
        date = ("0" + date_ob.getDate()).slice(-2),
        month = ("0" + (date_ob.getMonth() + 1)).slice(-2),
        year = date_ob.getFullYear(),
        hours = date_ob.getHours(),
        minutes = date_ob.getMinutes(),
        seconds = date_ob.getSeconds()
    
    // on recupere les informations de transactions depuis le front-end
    const newTransaction = { // en guise de 'Data binding' (en variable objet)
        codeSource: req.body.codesource,
        codeDestinataire: req.body.codedestinataire,
        montant: req.body.montant,
        dateTransfert: year + "-" + month + "-" + date + " " + hours + ":" + minutes + ":" + seconds
    }

    const id = req.user.id
    const query = `SELECT (codePermanent) FROM compte WHERE id = ${id}`
    // on identifie l'utilisateur dans la table 'compte' avec son "codePermanent"
    DBConnection.query(query, (err, data) => {
        if (err) return res.status(500).send(err)
        const query2 = `SELECT * FROM etudiants WHERE codePermanent = ${data[0].codePermanent}`
        // on recupère ses données en intégralité ..ensuite on les traite cas par cas en fonctions des
        // conditions de transactions solde > montant ; si le code du destinatire existe ? ; etc ...
        // ET update des 2 soldes ( source & destination ) à la fin de la transaction
        // ok !!  
        DBConnection.query(query2, (err, result) => {
            if (err) {
                return res.status(500).send(err)
            }
            if (result[0].codePermanent == newTransaction.codeSource) {
                if (result[0].solde <= newTransaction.montant) {
                    return req.session.sessionFlash = {
                        type: 'danger',
                        message: 'Compte insuffisant ! le montant à transférer est inférieur ou égal  à votre solde.'
                    },
                    res.redirect("/transfert")
                }
                // éviter de faire la transaction sur son propre compte
                if (newTransaction.codeSource == newTransaction.codeDestinataire) {
                    return req.session.sessionFlash = {
                        type: 'danger',
                        message: 'Attention ! Vous ne pouvez pas mettre votre code permanent comme destinataire.'
                    },
                    res.redirect("/transfert")
                }             
                // on vérifie si le "codePermanent" du destinataire existe réellement
                const checkCode = `SELECT (codePermanent) FROM etudiants WHERE codePermanent = ${newTransaction.codeDestinataire}`
                DBConnection.query(checkCode, (err, data) => {
                    if (err) {
                        return res.status(500).send(err)
                    }
                    if (data.length === 0 ) {
                        return req.session.sessionFlash = {
                            type: 'danger',
                            message: 'Le code permanent du destinataire ne correspond à aucun compte actif'
                        },
                        res.redirect("/transfert")
                        
                    }  else  {

                        const moinsLeMontant = `UPDATE etudiants SET solde = solde - ${newTransaction.montant} WHERE codePermanent = ${newTransaction.codeSource};`
                        DBConnection.query(moinsLeMontant, (err,_rows) => {
                        if (err) {
                            if (err) return res.status(500).send(err)
                        }
                        const ajoutMontant = `UPDATE etudiants SET solde = solde + ${newTransaction.montant} WHERE codePermanent = ${newTransaction.codeDestinataire};`
                            DBConnection.query(ajoutMontant, (err, _dt) => {
                                if (err) {
                                    if (err) return res.status(500).send(err)
                                } 
                                // On finalise la transaction en l'enregistrant dans sa table 
                                DBConnection.query('INSERT INTO transactions set ? ',newTransaction,( err,_result) => {
                                    if (err) throw err        
                                })
                            })
                        })

                        return req.session.sessionFlash = {
                            type: 'success',
                            message: 'Transfert éffectué avec succés !'
                        },
                        res.redirect("/transfert")
                    }
                })             
                /*
                const moinsLeMontant = `UPDATE etudiants SET solde = solde - ${newTransaction.montant} WHERE codePermanent = ${newTransaction.codeSource};`
                DBConnection.query(moinsLeMontant, (err,_rows) => {
                    if (err) {
                        if (err) return res.status(500).send(err)
                    }
                    const ajoutMontant = `UPDATE etudiants SET solde = solde + ${newTransaction.montant} WHERE codePermanent = ${newTransaction.codeDestinataire};`
                    DBConnection.query(ajoutMontant, (err, _dt) => {
                        if (err) {
                            if (err) return res.status(500).send(err)
                        } 
                        // On finalise la transaction en l'enregistrant dans sa table 
                        DBConnection.query('INSERT INTO transactions set ? ',newTransaction,( err,_result) => {
                            if (err) throw err        
                        })
                    })
                })

                return req.session.sessionFlash = {
                    type: 'success',
                    message: 'Transfert éffectué avec succés !'
                },
                res.redirect("/transfert")*/
                            
            } else {
                return req.session.sessionFlash = {
                    type: 'danger',
                    message: 'Erreur !! Entrer votre code permanent'
                },
                res.redirect("/transfert")
            }
        })
    })
}

module.exports = {
    getpageTransfert: getpageTransfert,
    transfert: transfert
}