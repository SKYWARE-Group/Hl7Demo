# Abstract

This project aims to demonstrate the integration of third-party products with the laboratory information system iLab. **The integration is event-driven**, based on the HL7 v2.5.1 standard.

We will strictly follow recommendations of [IHE](https://www.ihe.net/). The relevant IHE workflow is [Inter Laboratory Workflow (ILW)](https://www.ihe.net/uploadedFiles/Documents/Laboratory/IHE_LAB_Suppl_ILW.pdf), transactions **LAB-35** and **LAB-36**.

LIS iLab may be used both as **Requester** and **Subcontractor**. In many scenarios, it will cover both roles simultaneously with many partner organizations.

## Inter Laboratory Workflow

Here is the exemplary diagram of the workflow.

```mermaid
---
title: Inter Laboratory Workflow
---
sequenceDiagram

    actor Patient
    participant Requester
    participant Subcontractor

    Patient-->>+Requester: New Order
    Requester-->>-Patient: OK

    Note over Requester,Subcontractor: Initial order
    Requester->>+Subcontractor: OML_O21
    Subcontractor->>-Requester: ACK

    Patient-->>+Requester: Order Update
    Requester-->>-Patient: OK

    Note over Requester,Subcontractor: Order update
    Requester->>+Subcontractor: OML_O21
    Subcontractor->>-Requester: ACK

    Note over Requester,Subcontractor: Status change
    Subcontractor->>+Requester: OML_O21
    Requester->>-Subcontractor: ACK

    Note over Requester,Subcontractor: Results report
    Subcontractor->>+Requester: ORU_R01
    Requester->>-Subcontractor: ACK

    Patient-->>+Requester: Request results
    Requester-->>-Patient: Results Report
```
