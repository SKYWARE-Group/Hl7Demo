# Ordering

Ordering is an implementation of transaction **LAB-35** Sub-order Management. This transaction is used by the **Requester** to place orders to the **Subcontractor**. The transaction enables both **Requester** and **Subcontractor** to notify all subsequent changes of status and/or content of the order. The orders management uses **OML_O21** messages (Unsolicited Transmission of an Observation Message) and expects **ORL_O22** (General Laboratory Order Response) as a response.

## Requester role

As a **Requester**, LIS iLab will send orders via HL7 channel in real-time, based on the following events:

- When **order is created** in LIS iLab (this include manual registration or import from external systems)
- When **order is modified** in LIS iLab (either manually or programmatically)

LIS iLab will track already sent orders, so in case of modification it will send only required changes and will reference initially sent order. In this way, receiving party will have a real-time synchronization with iLab.

LIS iLab will receive order status change as well as result report messages.

## Subcontractor role

As a **Subcontractor**, LIS iLab will receive initial orders and order update messages.

During order processing, LIS iLab will send to the **Requester** status update messages.

When results are available, LIS iLab will send results report messages. Incremental (when an examination is ready) or "when whole order is complete" modes are available via channel settings.
