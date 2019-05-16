import React, { Component } from 'react'
import NavButton from "../navButton/NavButton"


class ComputersView extends Component {

    state = {
        nameSearchTerms: "",
        idSearchTerms: null
    }

    handleNameFieldChange = evt => {
        const stateToChange = {}
        stateToChange.nameSearchTerms = evt.target.value
        this.setState(stateToChange)
    }

    handleIdFieldChange = evt => {
        const stateToChange = {}
        stateToChange.idSearchTerms = parseInt(evt.target.value)
        if (evt.target.value === "") {
            stateToChange.idSearchTerms = null;
        }
        this.setState(stateToChange)
    }

    dateFormatter = date => {
        return date.split("T")[0]
    }

    render() {
        return (
            <React.Fragment>
                <div className="resourceView">
                    <div className="messageText">Computers</div>
                    <input type="text" spellCheck="false" autoComplete="off" className="input" onChange={this.handleNameFieldChange} placeholder="Search name"></input>
                    <input type="number" min="1" spellCheck="false" autoComplete="off" className="input" onChange={this.handleIdFieldChange} placeholder="Search ID"></input>
                    <div className="resourceContainer">
                        {
                            this.props.computers
                                .filter(c => c.make.toUpperCase().includes(this.state.nameSearchTerms.toUpperCase()) || c.manufacturer.toUpperCase().includes(this.state.nameSearchTerms.toUpperCase()))
                                .filter(c => (this.state.idSearchTerms !== null) ? c.id === this.state.idSearchTerms : c)
                                .map(computer => {
                                    return (
                                        <div key={computer.id} className="resource">
                                            <div><span className="darkerText">ID: </span><strong>{computer.id}</strong></div>
                                            <div><span className="darkerText">Name: </span><strong>{computer.manufacturer} {computer.make}</strong></div>
                                            <div><span className="darkerText">Purchase Date: </span><strong>{this.dateFormatter(computer.purchaseDate)}</strong></div>

                                            <div className="editButton" onClick={() => this.props.history.push("/computers/" + computer.id + "/edit")}>Edit</div>
                                        </div>
                                    )
                                })
                        }
                    </div>
                    <NavButton text={"Return"} history={this.props.history} path={"/"} />
                    <NavButton text={"Add New Computer"} history={this.props.history} path={"/computers/add"} />
                </div>
            </React.Fragment>
        )
    }
}

export default ComputersView