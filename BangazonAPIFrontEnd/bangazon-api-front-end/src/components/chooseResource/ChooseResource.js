import React, { Component } from 'react'


class ChooseResource extends Component {
    render() {
        return (
            <div className="resourceView">
                <div className="messageText">Choose a resource</div>
                <div className="resourceContainer">
                    <div className="category" onClick={() => this.props.history.push("/employees")}>Employees</div>
                    <div className="category" onClick={() => this.props.history.push("/departments")}>Departments</div>
                    <div className="category" onClick={() => this.props.history.push("/products")}>Products</div>
                    <div className="category" onClick={() => this.props.history.push("/producttypes")}>Product Types</div>
                    <div className="category" onClick={() => this.props.history.push("/orders")}>Orders</div>
                    <div className="category" onClick={() => this.props.history.push("/paymenttypes")}>Payment Types</div>
                    <div className="category" onClick={() => this.props.history.push("/customers")}>Customers</div>
                    <div className="category" onClick={() => this.props.history.push("/trainingprograms")}>Training Programs</div>
                    <div className="category" onClick={() => this.props.history.push("/computers")}>Computers</div>
                </div>
            </div>
        )
    }
}

export default ChooseResource