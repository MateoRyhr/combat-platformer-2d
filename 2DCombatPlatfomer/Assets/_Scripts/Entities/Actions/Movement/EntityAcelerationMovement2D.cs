using UnityEngine;

public class EntityAcelerationMovement2D : EntityMovement
{
    [Header("Movement config")]
    [SerializeField] private FloatVariable maxSpeed;
    // [SerializeField] private FloatVariable movementForce;
    [SerializeField] private AnimationCurve acelerationCurve;
    [SerializeField] private FloatVariable timeToReachMaxSpeed;
    [SerializeField] private AnimationCurve decelerationCurve;
    [SerializeField] private FloatVariable timeToSlowDown;
    [Header("Unit data")]
    [SerializeField] private EntityMovementController2D control;
    [SerializeField] private EntityStatus2D entityStatus;
    [SerializeField] private Rigidbody2D rigidBody;

    private float _timeAccelerating;
    private float _timeSlowingDown;

    private void FixedUpdate()
    {
        if(control.Direction != Vector2.zero) Move();
        else _timeAccelerating = 0f;
        if(entityStatus.IsOnGround() && !entityStatus.IsBeingAffectedByAnExternalForce) Stop();            
    }

    public override void Move()
    {
        Vector2 newVelocity = Vector2.Lerp(
            Vector2.zero,
            control.Direction.normalized * maxSpeed.Value,
            acelerationCurve.Evaluate(_timeAccelerating / timeToReachMaxSpeed.Value)
        );
        rigidBody.velocity = new Vector2(newVelocity.x,rigidBody.velocity.y);
        _timeAccelerating += Time.fixedDeltaTime;
        if(_timeAccelerating > timeToReachMaxSpeed.Value) _timeAccelerating = timeToReachMaxSpeed.Value;
    }

    public override void Stop(){
        Vector2 newVelocity = Vector2.Lerp(
            rigidBody.velocity,
            Vector2.zero,
            acelerationCurve.Evaluate(_timeSlowingDown / timeToSlowDown.Value)
        );
        rigidBody.velocity = rigidBody.velocity = new Vector2(newVelocity.x,rigidBody.velocity.y);
        if(control.Direction != Vector2.zero) _timeSlowingDown = 0f;
        else _timeSlowingDown += Time.fixedDeltaTime;
        if(_timeSlowingDown > timeToSlowDown.Value) _timeSlowingDown = timeToSlowDown.Value;
    }
}