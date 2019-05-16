import React, { Component } from 'react'


class NavButton extends Component {
    render() {
        return (
            <div className="navButton" onClick={() => this.props.history.push(this.props.path) }>{this.props.text}</div>
        )
    }
}

export default NavButton